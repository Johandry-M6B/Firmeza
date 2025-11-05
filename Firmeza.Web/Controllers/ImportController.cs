using Firmeza.Web.Data.Entities;
using Firmeza.Web.Models;
using Firmeza.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Web.Controllers;
[Authorize(Roles = UserRoles.Admin)]
public class ImportController: Controller
{
    private readonly IExcelImportService _excelImportService;

    public ImportController(IExcelImportService excelImportService)
    {
        _excelImportService = excelImportService;
    }

    // GET: Import
    public IActionResult Products()
    {
        return View(new ImportProductsViewModel());
    }
    // POST: Import
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Products(ImportProductsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.ExcelFile == null || model.ExcelFile.Length == 0)
        {
            ModelState.AddModelError("ExcelFile", "Please upload a valid Excel file.");
            return View(model);
        }

        if (model.ExcelFile.Length > 5 * 1024 * 1024)
        {
            ModelState.AddModelError("ExcelFile", "The file size exceeds the 5MB limit.");
            return View(model);
        }

        try
        {
        using var stream = model.ExcelFile.OpenReadStream();
        var result = await _excelImportService.ImportProductsFromExcel(stream);
        
        TempData["SuccessMessage"] = $"{result.TotalProcessed} products imported successfully. {result.TotalSuccess} products failed to import. Errors: {result.ErrorCount}";

        return View(result);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred while processing the file: {ex.Message}");
            return View(model);
        }
    }
    // GET: Import/DownloadTemplate
    public IActionResult DownloadTemplate()
    {
        var fileBytes = _excelImportService.GenerateProductTemplate();
        var fileName = $"Products_Template_{DateTime.Now:yyyyMMdd}.xlsx";
        return File(fileBytes, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            fileName);  
    }
}