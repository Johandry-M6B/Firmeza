

using Application.Categories.Queries.GetCategories;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.DeleteProduct;
using Application.Products.Queries.GetMeasurements;
using Application.Products.Queries.GetProductById;
using Application.Products.Queries.GetProducts;
using Firmeza.Application.Products.Commands.UpdateProduct;
using Firmeza.Application.Suppliers.Queries.GetSuppliers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MediatR;

using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class ProductsController : Controller
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: Products
    public async Task<IActionResult> Index(string searchTerm, int? categoryId)
    {
        var query = new GetProductsQuery
        {
            SearchTerm = searchTerm,
            CategoryId = categoryId,
            OnlyActive = true
        };

        var products = await _mediator.Send(query);
        
        // Para el filtro
        ViewBag.Categories = await GetCategoriesSelectList();
        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedCategoryId = categoryId;
        
        return View(products);
    }

    // GET: Products/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var query = new GetProductByIdQuery(id.Value);
        var product = await _mediator.Send(query);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Products/Create
    public async Task<IActionResult> Create()
    {
        await LoadSelectLists();
        return View();
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectLists();
            return View(command);
        }

        try
        {
            var productId = await _mediator.Send(command);
            TempData["SuccessMessage"] = "Producto creado exitosamente";
            return RedirectToAction(nameof(Details), new { id = productId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            await LoadSelectLists();
            return View(command);
        }
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var query = new GetProductByIdQuery(id.Value);
        var product = await _mediator.Send(query);

        if (product == null)
        {
            return NotFound();
        }

        // Mapear ProductDto a UpdateProductCommand
        var command = new UpdateProductCommand
        {
            Id = product.Id,
            Code = product.Code,
            Name = product.Name,
            Description = product.Description,
            CategoryId = product.CategoryId,
            MeasurementId = product.MeasurementId,
            SupplierId = product.SupplierId,
            BuyerPrice = product.BuyerPrice,
            SalePrice = product.SalePrice,
            WholesalePrice = product.WholesalePrice,
            MinimumStock = product.MinimumStock,
            Mark = product.Mark,
            Model = product.Model,
            Color = product.Color,
            Weight = product.Weight,
            Size = product.Size,
            RequiredRefrigeration = product.RequiredRefrigeration,
            DangerousMaterial = product.DangerousMaterial
        };

        await LoadSelectLists();
        return View(command);
    }

    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateProductCommand command)
    {
        if (id != command.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await LoadSelectLists();
            return View(command);
        }

        try
        {
            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Producto actualizado exitosamente";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            await LoadSelectLists();
            return View(command);
        }
    }

    // GET: Products/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var query = new GetProductByIdQuery(id.Value);
        var product = await _mediator.Send(query);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var command = new DeleteProductCommand { Id = id };
            await _mediator.Send(command);
            
            TempData["SuccessMessage"] = "Producto desactivado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    // ===================================
    // MÉTODOS PRIVADOS AUXILIARES
    // ===================================

    private async Task LoadSelectLists()
    {
        ViewData["CategoryId"] = await GetCategoriesSelectList();
        ViewData["MeasurementId"] = await GetMeasurementsSelectList();
        ViewData["SupplierId"] = await GetSuppliersSelectList();
    }

    private async Task<SelectList> GetCategoriesSelectList()
    {
        var categories = await _mediator.Send(new GetCategoriesQuery { OnlyActive = true });
        return new SelectList(categories, "Id", "Name");
    }

    private async Task<SelectList> GetMeasurementsSelectList()
    {
        var measurements = await _mediator.Send(new GetMeasurementsQuery { OnlyActive = true });
        return new SelectList(measurements, "Id", "Name");
    }

    private async Task<SelectList> GetSuppliersSelectList()
    {
        var suppliers = await _mediator.Send(new GetSuppliersQuery { OnlyActive = true });
        return new SelectList(suppliers, "Id", "TradeName");
    }
}