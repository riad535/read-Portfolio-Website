using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Features.Projects.Commands.CreateProject;
using PortfolioWebsite.Application.Features.Projects.Commands.DeleteProject;
using PortfolioWebsite.Application.Features.Projects.Commands.SetProjectPublished;
using PortfolioWebsite.Application.Features.Projects.Commands.UpdateProject;
using PortfolioWebsite.Application.Features.Projects.Queries.GetProjectById;
using PortfolioWebsite.Application.Features.Projects.Queries.GetProjects;
using PortfolioWebsite.Web.Areas.Admin.Models;

namespace PortfolioWebsite.Web.Areas.Admin.Controllers;

[Route("admin/projects")]
public sealed class ProjectsController : AdminControllerBase
{
    private readonly ISender _sender;

    public ProjectsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var projects = await _sender.Send(new GetProjectsQuery(), cancellationToken);

        ViewData["Title"] = "Projects";

        return View(projects);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var projects = await _sender.Send(new GetProjectsQuery(), cancellationToken);

        ViewData["Title"] = "New project";
        SetFormUrls("Create", null);

        return View("Form", new ProjectFormModel
        {
            DisplayOrder = GetNextDisplayOrder(projects)
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(
        ProjectFormModel model,
        CancellationToken cancellationToken)
    {
        ViewData["Title"] = "New project";
        SetFormUrls("Create", null);

        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        try
        {
            await _sender.Send(
                new CreateProjectCommand(
                    (model.Title ?? string.Empty).Trim(),
                    (model.Description ?? string.Empty).Trim(),
                    (model.TechStack ?? string.Empty).Trim(),
                    AdminFormHelpers.NormalizeOptional(model.Architecture),
                    AdminFormHelpers.NormalizeOptional(model.LiveDemoUrl),
                    AdminFormHelpers.NormalizeOptional(model.GitHubUrl),
                    model.DisplayOrder,
                    model.IsPublished),
                cancellationToken);

            TempData["SuccessMessage"] = "Project saved.";

            return RedirectToAction(nameof(Index));
        }
        catch (ValidationException exception)
        {
            AdminFormHelpers.AddValidationErrors<ProjectFormModel>(ModelState, exception);

            return View("Form", model);
        }
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit project";
        SetFormUrls("Edit", id);

        try
        {
            var project = await _sender.Send(new GetProjectByIdQuery(id), cancellationToken);

            return View("Form", new ProjectFormModel
            {
                Title = project.Title,
                Description = project.Description,
                TechStack = project.TechStack,
                Architecture = project.Architecture,
                LiveDemoUrl = project.LiveDemoUrl,
                GitHubUrl = project.GitHubUrl,
                DisplayOrder = project.DisplayOrder,
                IsPublished = project.IsPublished
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
        ProjectFormModel model,
        CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit project";
        SetFormUrls("Edit", id);

        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        try
        {
            await _sender.Send(
                new UpdateProjectCommand(
                    id,
                    (model.Title ?? string.Empty).Trim(),
                    (model.Description ?? string.Empty).Trim(),
                    (model.TechStack ?? string.Empty).Trim(),
                    AdminFormHelpers.NormalizeOptional(model.Architecture),
                    AdminFormHelpers.NormalizeOptional(model.LiveDemoUrl),
                    AdminFormHelpers.NormalizeOptional(model.GitHubUrl),
                    model.DisplayOrder,
                    model.IsPublished),
                cancellationToken);

            TempData["SuccessMessage"] = "Project saved.";

            return RedirectToAction(nameof(Index));
        }
        catch (ValidationException exception)
        {
            AdminFormHelpers.AddValidationErrors<ProjectFormModel>(ModelState, exception);

            return View("Form", model);
        }
        catch (NotFoundException)
        {
            TempData["ErrorMessage"] = "This project no longer exists.";

            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _sender.Send(new DeleteProjectCommand(id), cancellationToken);

            TempData["SuccessMessage"] = "Project deleted.";
        }
        catch (ValidationException exception)
        {
            TempData["ErrorMessage"] = AdminFormHelpers.GetFirstErrorMessage(exception);
        }
        catch (NotFoundException)
        {
            TempData["ErrorMessage"] = "This project no longer exists.";
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
                new SetProjectPublishedCommand(id, isPublished),
                cancellationToken);

            TempData["SuccessMessage"] = "Status updated.";
        }
        catch (NotFoundException)
        {
            TempData["ErrorMessage"] = "This project no longer exists.";
        }

        return RedirectToAction(nameof(Index));
    }

    private void SetFormUrls(string action, int? id)
    {
        ViewData["FormActionUrl"] = Url.Action(action, "Projects", new { area = "Admin", id });
        ViewData["CancelUrl"] = Url.Action(nameof(Index), "Projects", new { area = "Admin" });
    }

    private static int GetNextDisplayOrder(IReadOnlyList<ProjectListItemDto> projects)
    {
        return projects.Count == 0
            ? 0
            : projects.Max(project => project.DisplayOrder) + 1;
    }
}
