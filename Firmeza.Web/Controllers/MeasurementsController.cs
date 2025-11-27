


using Application.Measurements.Commands.DeleteMeasurements;
using Application.Measurements.Commands.UpdateMeasurements;
using Application.Measurements.Queries.CreateMeasurement;
using Application.Measurements.Queries.GetMeasurementById;
using Application.Products.Queries.GetMeasurements;
using Domain.Enums;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Web.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class MeasurementsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;

    public MeasurementsController(
        ApplicationDbContext context,
        IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var query = new GetMeasurementsQuery { OnlyActive = false };
        var measurements = await _mediator.Send(query);
        return View(measurements);
    }

    public async Task<IActionResult> Details(int id)
    {
        var query = new GetMeasurementByIdQuery(id);
        var measurement = await _mediator.Send(query);

        if (measurement == null)
        {
            return NotFound();
        }

        return View(measurement);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMeasurementCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }

        try
        {
            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Unidad de medida creada exitosamente";
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
        var measurement = await _context.Measurements.FindAsync(id);
        if (measurement == null)
        {
            return NotFound();
        }

    return View(measurement);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateMeasurementCommand command)
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
            TempData["SuccessMessage"] = "Unidad de medida actualizada exitosamente";
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
        var query = new GetMeasurementByIdQuery(id);
        var measurement = await _mediator.Send(query);

        if (measurement == null)
        {
            return NotFound();
        }

        return View(measurement);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var command = new DeleteMeasurementCommand { Id = id };
            await _mediator.Send(command);
            
            TempData["SuccessMessage"] = "Unidad de medida eliminada exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}