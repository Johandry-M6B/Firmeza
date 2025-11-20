// Firmeza.Web/Controllers/CategoriesController.cs

using Application.Categories.Queries.GetCategories;
using Application.Categories.Queries.GetCategoryById;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Firmeza.Application.Categories.Commands.UpdateCategory;
using Firmeza.Application.Categories.Commands.DeleteCategory;
using Firmeza.Application.Categories.Queries.GetCategories;
using Firmeza.Application.Categories.Queries.GetCategoryById;
using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class CategoriesController : Controller
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: Categories
    public async Task<IActionResult> Index()
    {
        var query = new GetCategoriesQuery { OnlyActive = false };
        var categories = await _mediator.Send(query);
        return View(categories);
    }

    // GET: Categories/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var query = new GetCategoryByIdQuery(id);
        var category = await _mediator.Send(query);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    // GET: Categories/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Categories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategoryCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }

        try
        {
            var categoryId = await _mediator.Send(command);
            TempData["SuccessMessage"] = "Categoría creada exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(command);
        }
    }

    // GET: Categories/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var query = new GetCategoryByIdQuery(id);
        var category = await _mediator.Send(query);

        if (category == null)
        {
            return NotFound();
        }

        var command = new UpdateCategoryCommand
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };

        return View(command);
    }

    // POST: Categories/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateCategoryCommand command)
    {
        if (id != command.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(command);
        }

        try
        {
            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Categoría actualizada exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(command);
        }
    }

    // GET: Categories/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var query = new GetCategoryByIdQuery(id);
        var category = await _mediator.Send(query);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    // POST: Categories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var command = new DeleteCategoryCommand { Id = id };
            await _mediator.Send(command);
            
            TempData["SuccessMessage"] = "Categoría eliminada exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}