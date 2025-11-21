
using Application.Customers.Queries.GetCustomers;
using Application.Products.Queries.GetProducts;
using Application.Sales.Commands.AddPayment;
using Application.Sales.Commands.CancelSale;
using Application.Sales.Commands.CreateSale;
using Application.Sales.Queries.GetSaleById;
using Application.Sales.Queries.GetSales;
using Domain.Enums;
using Firmeza.Web.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Firmeza.Web.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class SalesController : Controller
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: Sales
    public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, int? customerId)
    {
        var query = new GetSalesQuery
        {
            StartDate = startDate,
            EndDate = endDate,
            CustomerId = customerId
        };

        var sales = await _mediator.Send(query);
        
        // Cargar clientes para filtro
        var customers = await _mediator.Send(new GetCustomersQuery { OnlyActive = true });
        ViewBag.Customers = new SelectList(customers, "Id", "FullName");
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;
        ViewBag.SelectedCustomerId = customerId;
        
        return View(sales);
    }

    // GET: Sales/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var query = new GetSaleByIdQuery(id);
        var sale = await _mediator.Send(query);

        if (sale == null)
        {
            return NotFound();
        }

        return View(sale);
    }

    // GET: Sales/Create
    public async Task<IActionResult> Create()
    {
        await LoadSelectLists();
        return View();
    }

    // POST: Sales/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSaleCommand command)
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectLists();
            return View(command);
        }

        try
        {
            var saleId = await _mediator.Send(command);
            TempData["SuccessMessage"] = "Venta registrada exitosamente";
            return RedirectToAction(nameof(Details), new { id = saleId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            await LoadSelectLists();
            return View(command);
        }
    }

    // POST: Sales/Cancel/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, string reason)
    {
        try
        {
            var command = new CancelSaleCommand
            {
                SaleId = id,
                Reason = reason
            };
            
            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Venta cancelada exitosamente";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    // POST: Sales/AddPayment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPayment(AddPaymentCommand command)
    {
        try
        {
            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Pago registrado exitosamente";
            return RedirectToAction(nameof(Details), new { id = command.SaleId });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Details), new { id = command.SaleId });
        }
    }

    private async Task LoadSelectLists()
    {
        var customers = await _mediator.Send(new GetCustomersQuery { OnlyActive = true });
        var products = await _mediator.Send(new GetProductsQuery { OnlyActive = true });
        
        ViewBag.Customers = new SelectList(customers, "Id", "FullName");
        ViewBag.Products = new SelectList(products, "Id", "Name");
    }
}