using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Features.Skills.Commands.CreateSkillCategory;
using PortfolioWebsite.Application.Features.Skills.Commands.DeleteSkillCategory;
using PortfolioWebsite.Application.Features.Skills.Commands.UpdateSkillCategory;
using PortfolioWebsite.Application.Features.Skills.Queries.GetSkillCategories;
using PortfolioWebsite.Application.Features.Skills.Queries.GetSkillCategoryById;
using PortfolioWebsite.Web.Areas.Admin.Models;

namespace PortfolioWebsite.Web.Areas.Admin.Controllers;

[Route("admin/skills")]
public sealed class SkillsController : AdminControllerBase
{
    private readonly ISender _sender;

    public SkillsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var categories = await _sender.Send(new GetSkillCategoriesQuery(), cancellationToken);

        ViewData["Title"] = "Skills";

        return View(categories);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var categories = await _sender.Send(new GetSkillCategoriesQuery(), cancellationToken);

        ViewData["Title"] = "New skill category";
        SetFormUrls("Create", null);

        return View("Form", new SkillCategoryFormModel
        {
            DisplayOrder = GetNextDisplayOrder(categories)
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(
        SkillCategoryFormModel model,
        CancellationToken cancellationToken)
    {
        ViewData["Title"] = "New skill category";
        SetFormUrls("Create", null);

        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        try
        {
            await _sender.Send(
                new CreateSkillCategoryCommand(
                    (model.Name ?? string.Empty).Trim(),
                    (model.IconClass ?? string.Empty).Trim(),
                    model.DisplayOrder,
                    AdminFormHelpers.SplitLines(model.Skills)),
                cancellationToken);

            TempData["SuccessMessage"] = "Skill category saved.";

            return RedirectToAction(nameof(Index));
        }
        catch (ValidationException exception)
        {
            AdminFormHelpers.AddValidationErrors<SkillCategoryFormModel>(ModelState, exception);

            return View("Form", model);
        }
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit skill category";
        SetFormUrls("Edit", id);

        try
        {
            var category = await _sender.Send(
                new GetSkillCategoryByIdQuery(id),
                cancellationToken);

            return View("Form", new SkillCategoryFormModel
            {
                Name = category.Name,
                IconClass = category.IconClass,
                DisplayOrder = category.DisplayOrder,
                Skills = string.Join("\n", category.Skills)
            });
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("edit/{id:int}")]
    public async Task<IActionResult> Edit(
        int id,
        SkillCategoryFormModel model,
        CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit skill category";
        SetFormUrls("Edit", id);

        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        try
        {
            await _sender.Send(
                new UpdateSkillCategoryCommand(
                    id,
                    (model.Name ?? string.Empty).Trim(),
                    (model.IconClass ?? string.Empty).Trim(),
                    model.DisplayOrder,
                    AdminFormHelpers.SplitLines(model.Skills)),
                cancellationToken);

            TempData["SuccessMessage"] = "Skill category saved.";

            return RedirectToAction(nameof(Index));
        }
        catch (ValidationException exception)
        {
            AdminFormHelpers.AddValidationErrors<SkillCategoryFormModel>(ModelState, exception);

            return View("Form", model);
        }
        catch (NotFoundException)
        {
            TempData["ErrorMessage"] = "This skill category no longer exists.";

            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _sender.Send(new DeleteSkillCategoryCommand(id), cancellationToken);

            TempData["SuccessMessage"] = "Skill category deleted.";
        }
        catch (ValidationException exception)
        {
            TempData["ErrorMessage"] = AdminFormHelpers.GetFirstErrorMessage(exception);
        }
        catch (NotFoundException)
        {
            TempData["ErrorMessage"] = "This skill category no longer exists.";
        }

        return RedirectToAction(nameof(Index));
    }

    private void SetFormUrls(string action, int? id)
    {
        ViewData["FormActionUrl"] = Url.Action(action, "Skills", new { area = "Admin", id });
        ViewData["CancelUrl"] = Url.Action(nameof(Index), "Skills", new { area = "Admin" });
    }

    private static int GetNextDisplayOrder(IReadOnlyList<SkillCategoryListItemDto> categories)
    {
        return categories.Count == 0
            ? 0
            : categories.Max(category => category.DisplayOrder) + 1;
    }
}
