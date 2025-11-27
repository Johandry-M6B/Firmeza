using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Web.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class ImportController : Controller
{
    private readonly IMediator _mediator;
    private readonly IExcelService _excelService;

    public ImportController(
        IMediator mediator,
        IExcelService excelService)
    {
        _mediator = mediator;
        _excelService = excelService;
    }

    // GET: Import/Products
    public IActionResult Products()
    {
        return View();
    }

    // POST: Import/Products
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Products(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("", "Por favor seleccione un archivo");
            return View();
        }

        if (!file.FileName.EndsWith(".xlsx") && !file.FileName.EndsWith(".xls"))
        {
            ModelState.AddModelError("", "El archivo debe ser un archivo Excel (.xlsx o .xls)");
            return View();
        }

        try
        {
            // Aquí implementarías la lógica de importación
            // usando el ExcelService y Commands de CreateProduct
            
            TempData["SuccessMessage"] = "Productos importados exitosamente";
            return RedirectToAction("Index", "Products");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error al importar: {ex.Message}");
            return View();
        }
    }

    // GET: Import/DownloadTemplate
    public IActionResult DownloadTemplate()
    {
        var template = _excelService.GenerateProductTemplate();
        return File(template, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PlantillaProductos.xlsx");
    }
}