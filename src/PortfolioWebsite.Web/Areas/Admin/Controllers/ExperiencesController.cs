using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Features.Experiences.Commands.CreateExperience;
using PortfolioWebsite.Application.Features.Experiences.Commands.DeleteExperience;
using PortfolioWebsite.Application.Features.Experiences.Commands.SetExperiencePublished;
using PortfolioWebsite.Application.Features.Experiences.Commands.UpdateExperience;
using PortfolioWebsite.Application.Features.Experiences.Queries.GetExperienceById;
using PortfolioWebsite.Application.Features.Experiences.Queries.GetExperiences;
using PortfolioWebsite.Web.Areas.Admin.Models;

namespace PortfolioWebsite.Web.Areas.Admin.Controllers;

[Route("admin/experiences")]
public sealed class ExperiencesController : AdminControllerBase
{
    private readonly ISender _sender;

    public ExperiencesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var experiences = await _sender.Send(new GetExperiencesQuery(), cancellationToken);

        ViewData["Title"] = "Experience";

        return View(experiences);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewData["Title"] = "New experience";
        SetFormUrls("Create", null);

        return View("Form", new ExperienceFormModel());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(
        ExperienceFormModel model,
        CancellationToken cancellationToken)
    {
        ViewData["Title"] = "New experience";
        SetFormUrls("Create", null);

        if (!TryBuildDates(model, out var startDate, out var endDate))
        {
            return View("Form", model);
        }

        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        try
        {
            await _sender.Send(
                new CreateExperienceCommand(
                    (model.Role ?? string.Empty).Trim(),
                    (model.Organisation ?? string.Empty).Trim(),
                    startDate,
                    endDate,
                    model.IsCurrent,
                    model.IsPublished,
                    AdminFormHelpers.SplitLines(model.Points)),
                cancellationToken);

            TempData["SuccessMessage"] = "Experience saved.";

            return RedirectToAction(nameof(Index));
        }
        catch (ValidationException exception)
        {
            AdminFormHelpers.AddValidationErrors<ExperienceFormModel>(ModelState, exception);

            return View("Form", model);
        }
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit experience";
        SetFormUrls("Edit", id);

        try
        {
            var experience = await _sender.Send(
                new GetExperienceByIdQuery(id),
                cancellationToken);

            return View("Form", new ExperienceFormModel
            {
                Role = experience.Role,
                Organisation = experience.Organisation,
                StartDate = AdminFormHelpers.FormatMonth(experience.StartDate),
                EndDate = experience.EndDate is null
                    ? null
                    : AdminFormHelpers.FormatMonth(experience.EndDate.Value),
                IsCurrent = experience.IsCurrent,
                Points = string.Join("\n", experience.Points),
                IsPublished = experience.IsPublished
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
        ExperienceFormModel model,
        CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit experience";
        SetFormUrls("Edit", id);

        if (!TryBuildDates(model, out var startDate, out var endDate))
        {
            return View("Form", model);
        }

        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        try
        {
            await _sender.Send(
                new UpdateExperienceCommand(
                    id,
                    (model.Role ?? string.Empty).Trim(),
                    (model.Organisation ?? string.Empty).Trim(),
                    startDate,
                    endDate,
                    model.IsCurrent,
                    model.IsPublished,
                    AdminFormHelpers.SplitLines(model.Points)),
                cancellationToken);

            TempData["SuccessMessage"] = "Experience saved.";

            return RedirectToAction(nameof(Index));
        }
        catch (ValidationException exception)
        {
            AdminFormHelpers.AddValidationErrors<ExperienceFormModel>(ModelState, exception);

            return View("Form", model);
        }
        catch (NotFoundException)
        {
            TempData["ErrorMessage"] = "This experience no longer exists.";

            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _sender.Send(new DeleteExperienceCommand(id), cancellationToken);

            TempData["SuccessMessage"] = "Experience deleted.";
        }
        catch (ValidationException exception)
        {
            TempData["ErrorMessage"] = AdminFormHelpers.GetFirstErrorMessage(exception);
        }
        catch (NotFoundException)
        {
            TempData["ErrorMessage"] = "This experience no longer exists.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("set-published/{id:int}")]
    public async Task<IActionResult> SetPublished(
        int id,
        bool isPublished,
        CancellationToken cancellationToken)
    {
        try
        {
            await _sender.Send(
                new SetExperiencePublishedCommand(id, isPublished),
                cancellationToken);

            TempData["SuccessMessage"] = "Status updated.";
        }
        catch (NotFoundException)
        {
            TempData["ErrorMessage"] = "This experience no longer exists.";
        }

        return RedirectToAction(nameof(Index));
    }

    private void SetFormUrls(string action, int? id)
    {
        ViewData["FormActionUrl"] = Url.Action(action, "Experiences", new { area = "Admin", id });
        ViewData["CancelUrl"] = Url.Action(nameof(Index), "Experiences", new { area = "Admin" });
    }

    private bool TryBuildDates(
        ExperienceFormModel model,
        out DateOnly startDate,
        out DateOnly? endDate)
    {
        startDate = default;
        endDate = null;
        var isValid = true;

        if (!string.IsNullOrWhiteSpace(model.StartDate))
        {
            if (AdminFormHelpers.TryParseMonth(model.StartDate, out var parsedStart))
            {
                startDate = parsedStart;
            }
            else
            {
                ModelState.AddModelError(
                    nameof(model.StartDate),
                    "Enter a valid start month.");
                isValid = false;
            }
        }

        if (!model.IsCurrent && !string.IsNullOrWhiteSpace(model.EndDate))
        {
            if (AdminFormHelpers.TryParseMonth(model.EndDate, out var parsedEnd))
            {
                endDate = parsedEnd;
            }
            else
            {
                ModelState.AddModelError(
                    nameof(model.EndDate),
                    "Enter a valid end month.");
                isValid = false;
            }
        }

        return isValid;
    }
}
