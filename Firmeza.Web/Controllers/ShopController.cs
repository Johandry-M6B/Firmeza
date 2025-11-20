// Firmeza.Web/Controllers/ShopController.cs

using Application.Categories.Queries.GetCategories;
using Application.Products.Queries.GetProductById;
using Application.Products.Queries.GetProducts;
using Microsoft.AspNetCore.Mvc;
using MediatR;


namespace Firmeza.Web.Controllers;

public class ShopController : Controller
{
    private readonly IMediator _mediator;

    public ShopController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(int? categoryId, string searchTerm)
    {
        var query = new GetProductsQuery
        {
            CategoryId = categoryId,
            SearchTerm = searchTerm,
            OnlyActive = true
        };

        var products = await _mediator.Send(query);
        
        // Cargar categorías para el filtro
        var categoriesQuery = new GetCategoriesQuery { OnlyActive = true };
        var categories = await _mediator.Send(categoriesQuery);
        
        ViewBag.Categories = categories;
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.SearchTerm = searchTerm;
        
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var query = new GetProductByIdQuery(id);
        var product = await _mediator.Send(query);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
}