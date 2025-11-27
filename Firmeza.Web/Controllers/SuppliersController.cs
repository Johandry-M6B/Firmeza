

using Application.Suppliers.Commands.CreateSuppliers;
using Application.Suppliers.Commands.DeleteSuppliers;
using Application.Suppliers.Commands.UpdateSupplier;
using Application.Suppliers.Queries.GetSupplierById;
using Domain.Enums;
using Firmeza.Application.Suppliers.Queries.GetSuppliers;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;



namespace Firmeza.Web.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class SuppliersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;

    public SuppliersController(
        ApplicationDbContext context,
        IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(string searchTerm)
    {
        var query = new GetSuppliersQuery
        {
            SearchTerm = searchTerm,
            OnlyActive = false
        };

        var suppliers = await _mediator.Send(query);
        ViewBag.SearchTerm = searchTerm;
        
        return View(suppliers);
    }

    public async Task<IActionResult> Details(int id)
    {
        var query = new GetSupplierByIdQuery(id);
        var supplier = await _mediator.Send(query);

        if (supplier == null)
        {
            return NotFound();
        }

        return View(supplier);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSupplierCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }

        try
        {
            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Proveedor creado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(command);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }
        
        return View(supplier);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateSupplierCommand command)
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
            TempData["SuccessMessage"] = "Proveedor actualizado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(command);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var query = new GetSupplierByIdQuery(id);
        var supplier = await _mediator.Send(query);

        if (supplier == null)
        {
            return NotFound();
        }

        return View(supplier);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var command = new DeleteSupplierCommand { Id = id };
            await _mediator.Send(command);
            
            TempData["SuccessMessage"] = "Proveedor eliminado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}