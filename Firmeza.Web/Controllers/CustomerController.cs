// Firmeza.Web/Controllers/CustomersController.cs

using Application.Customers.Commands.CreateCustomer;
using Application.Customers.Commands.DeleteCustomer;
using Application.Customers.Commands.UpdateCustomer;
using Application.Customers.Queries.GetCustomerById;
using Application.Customers.Queries.GetCustomers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Firmeza.Web.Data.Entities;


namespace Firmeza.Web.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class CustomersController : Controller
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(string searchTerm)
    {
        var query = new GetCustomersQuery
        {
            SearchTerm = searchTerm,
            OnlyActive = false
        };

        var customers = await _mediator.Send(query);
        ViewBag.SearchTerm = searchTerm;
        
        return View(customers);
    }

    public async Task<IActionResult> Details(int id)
    {
        var query = new GetCustomerByIdQuery(id);
        var customer = await _mediator.Send(query);

        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCustomerCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }

        try
        {
            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Cliente creado exitosamente";
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
        var query = new GetCustomerByIdQuery(id);
        var customer = await _mediator.Send(query);

        if (customer == null)
        {
            return NotFound();
        }

        var command = new UpdateCustomerCommand
        {
            Id = customer.Id,
            FullName = customer.FullName,
            DocumentNumber = customer.DocumentNumber,
            TypeCustomer = customer.TypeCustomer,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            City = customer.City,
            Country = customer.Country
        };

        return View(command);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateCustomerCommand command)
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
            TempData["SuccessMessage"] = "Cliente actualizado exitosamente";
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
        var query = new GetCustomerByIdQuery(id);
        var customer = await _mediator.Send(query);

        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var command = new DeleteCustomerCommand { Id = id };
            await _mediator.Send(command);
            
            TempData["SuccessMessage"] = "Cliente desactivado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}