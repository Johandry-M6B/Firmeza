using ClosedXML.Excel;
using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Models;

using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Services
{
    public interface IExcelImportService
    {
        Task<ImportProductsViewModel> ImportProductsFromExcel(Stream fileStream);
        byte[] GenerateProductTemplate();
    }

    public class ExcelImportService : IExcelImportService
    {
        private readonly ApplicationDbContext _context;

        public ExcelImportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ImportProductsViewModel> ImportProductsFromExcel(Stream fileStream)
        {
            var result = new ImportProductsViewModel();
            var importedProducts = new List<ProductImportDto>();

            try
            {
                using var workbook = new XLWorkbook(fileStream);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1); // Saltar encabezados

                // Leer datos del Excel
                foreach (var row in rows)
                {
                    try
                    {
                        var product = new ProductImportDto
                        {
                            Code = row.Cell(1).GetString(),
                            Name = row.Cell(2).GetString(),
                            Description = row.Cell(3).GetString(),
                            CategoryName = row.Cell(4).GetString(),
                            MeasurementName = row.Cell(5).GetString(),
                            SupplierName = row.Cell(6).GetString(),
                            BuyerPrice = row.Cell(7).GetValue<decimal>(),
                            SalePrice = row.Cell(8).GetValue<decimal>(),
                            WholesalePrice = row.Cell(9).GetValue<decimal>(),
                            CurrentStock = row.Cell(10).GetValue<int>(),
                            MinimumStock = row.Cell(11).GetValue<int>(),
                            Mark = row.Cell(12).GetString(),
                            Model = row.Cell(13).GetString(),
                            Color = row.Cell(14).GetString(),
                            Weight = string.IsNullOrEmpty(row.Cell(15).GetString()) ? null : row.Cell(15).GetValue<decimal?>(),
                            Size = row.Cell(16).GetString(),
                            RequiredRefrigeration = row.Cell(17).GetString().ToLower() == "si" || row.Cell(17).GetString().ToLower() == "yes",
                            DangerousMaterial = row.Cell(18).GetString().ToLower() == "si" || row.Cell(18).GetString().ToLower() == "yes"
                        };

                        importedProducts.Add(product);
                    }
                    catch (Exception ex)
                    {
                        result.Results.Add(new ProductImportResult
                        {
                            Row = row.RowNumber(),
                            Success = false,
                            Message = "Error al leer la fila",
                            ErrorDetails = ex.Message
                        });
                    }
                }

                // Procesar productos
                foreach (var productDto in importedProducts)
                {
                    var importResult = await ProcessProduct(productDto, importedProducts.IndexOf(productDto) + 2);
                    result.Results.Add(importResult);

                    if (importResult.Success)
                        result.TotalSuccess++;
                    else
                        result.ErrorCount++;
                }

                result.TotalProcessed = importedProducts.Count;
            }
            catch (Exception ex)
            {
                result.Results.Add(new ProductImportResult
                {
                    Success = false,
                    Message = "Error general al procesar el archivo",
                    ErrorDetails = ex.Message
                });
            }

            return result;
        }

        private async Task<ProductImportResult> ProcessProduct(ProductImportDto dto, int rowNumber)
        {
            var result = new ProductImportResult
            {
                Row = rowNumber,
                Code = dto.Code,
                ProductName = dto.Name
            };

            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(dto.Code))
                {
                    result.Success = false;
                    result.Message = "El código es requerido";
                    return result;
                }

                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    result.Success = false;
                    result.Message = "El nombre es requerido";
                    return result;
                }

                // Verificar si el producto ya existe
                var existingProduct = await _context.Products
                    .FirstOrDefaultAsync(p => p.Code == dto.Code);

                if (existingProduct != null)
                {
                    // Actualizar producto existente
                    existingProduct.Name = dto.Name;
                    existingProduct.Description = dto.Description;
                    existingProduct.BuyerPrice = dto.BuyerPrice;
                    existingProduct.SalePrice = dto.SalePrice;
                    existingProduct.WholesalePrice = dto.WholesalePrice;
                    existingProduct.CurrentStock += dto.CurrentStock; // Sumar al stock actual
                    existingProduct.MinimumStock = dto.MinimumStock;
                    existingProduct.Mark = dto.Mark;
                    existingProduct.Model = dto.Model;
                    existingProduct.Color = dto.Color;
                    existingProduct.Weight = dto.Weight;
                    existingProduct.Size = dto.Size;
                    existingProduct.RequiredRefrigeration = dto.RequiredRefrigeration;
                    existingProduct.DangerousMaterial = dto.DangerousMaterial;
                    existingProduct.DateUpdated = DateTime.UtcNow;

                    // Registrar movimiento de inventario
                    var movement = new InventoryMovement
                    {
                        ProductId = existingProduct.Id,
                        MovementType = MovementType.Input,
                        Date = DateTime.UtcNow,
                        Quantity = dto.CurrentStock,
                        AfterStock = existingProduct.CurrentStock - dto.CurrentStock,
                        NewStock = existingProduct.CurrentStock,
                        Observation = "Importación desde Excel"
                    };
                    _context.InventoryMovements.Add(movement);

                    result.Success = true;
                    result.Message = $"Producto actualizado. Stock aumentado en {dto.CurrentStock}";
                }
                else
                {
                    // Buscar o crear categoría
                    var category = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Name.ToLower() == dto.CategoryName.ToLower());

                    if (category == null)
                    {
                        category = new Category
                        {
                            Name = dto.CategoryName,
                            Active = true
                        };
                        _context.Categories.Add(category);
                        await _context.SaveChangesAsync();
                    }

                    // Buscar o crear unidad de medida
                    var measurement = await _context.Measurements
                        .FirstOrDefaultAsync(m => m.Name.ToLower() == dto.MeasurementName.ToLower());

                    if (measurement == null)
                    {
                        measurement = new Measurement
                        {
                            Name = dto.MeasurementName,
                            Abbreviation = dto.MeasurementName.Substring(0, Math.Min(3, dto.MeasurementName.Length)),
                            Active = true
                        };
                        _context.Measurements.Add(measurement);
                        await _context.SaveChangesAsync();
                    }

                    // Buscar proveedor (opcional)
                    Supplier? supplier = null;
                    if (!string.IsNullOrWhiteSpace(dto.SupplierName))
                    {
                        supplier = await _context.Suppliers
                            .FirstOrDefaultAsync(s => s.TradeName.ToLower() == dto.SupplierName.ToLower());
                    }

                    // Crear nuevo producto
                    var newProduct = new Product
                    {
                        Code = dto.Code,
                        Name = dto.Name,
                        Description = dto.Description,
                        CategoryId = category.Id,
                        MeasurementId = measurement.Id,
                        SupplierId = supplier?.Id,
                        BuyerPrice = dto.BuyerPrice,
                        SalePrice = dto.SalePrice,
                        WholesalePrice = dto.WholesalePrice,
                        CurrentStock = dto.CurrentStock,
                        MinimumStock = dto.MinimumStock,
                        Mark = dto.Mark,
                        Model = dto.Model,
                        Color = dto.Color,
                        Weight = dto.Weight,
                        Size = dto.Size,
                        RequiredRefrigeration = dto.RequiredRefrigeration,
                        DangerousMaterial = dto.DangerousMaterial,
                        Active = true
                    };

                    _context.Products.Add(newProduct);
                    await _context.SaveChangesAsync();

                    // Registrar movimiento de inventario inicial
                    var movement = new InventoryMovement
                    {
                        ProductId = newProduct.Id,
                        MovementType = MovementType.Input,
                        Date = DateTime.UtcNow,
                        Quantity = dto.CurrentStock,
                        AfterStock = 0,
                        NewStock = dto.CurrentStock,
                        Observation = "Stock inicial - Importación desde Excel"
                    };
                    _context.InventoryMovements.Add(movement);

                    result.Success = true;
                    result.Message = "Producto creado exitosamente";
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al procesar el producto";
                result.ErrorDetails = ex.Message;
            }

            return result;
        }

        public byte[] GenerateProductTemplate()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Productos");

            // Encabezados
            worksheet.Cell(1, 1).Value = "Código*";
            worksheet.Cell(1, 2).Value = "Nombre*";
            worksheet.Cell(1, 3).Value = "Descripción";
            worksheet.Cell(1, 4).Value = "Categoría*";
            worksheet.Cell(1, 5).Value = "Unidad Medida*";
            worksheet.Cell(1, 6).Value = "Proveedor";
            worksheet.Cell(1, 7).Value = "Precio Compra*";
            worksheet.Cell(1, 8).Value = "Precio Venta*";
            worksheet.Cell(1, 9).Value = "Precio Mayorista*";
            worksheet.Cell(1, 10).Value = "Stock Actual*";
            worksheet.Cell(1, 11).Value = "Stock Mínimo*";
            worksheet.Cell(1, 12).Value = "Marca";
            worksheet.Cell(1, 13).Value = "Modelo";
            worksheet.Cell(1, 14).Value = "Color";
            worksheet.Cell(1, 15).Value = "Peso (Kg)";
            worksheet.Cell(1, 16).Value = "Dimensiones";
            worksheet.Cell(1, 17).Value = "Refrigeración (Si/No)";
            worksheet.Cell(1, 18).Value = "Peligroso (Si/No)";

            // Formato de encabezados
            var headerRange = worksheet.Range(1, 1, 1, 18);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Ejemplos de datos
            worksheet.Cell(2, 1).Value = "PROD001";
            worksheet.Cell(2, 2).Value = "Cemento Gris 50kg";
            worksheet.Cell(2, 3).Value = "Cemento de construcción uso general";
            worksheet.Cell(2, 4).Value = "Cemento";
            worksheet.Cell(2, 5).Value = "Bolsa";
            worksheet.Cell(2, 6).Value = "Cementos SA";
            worksheet.Cell(2, 7).Value = 25000;
            worksheet.Cell(2, 8).Value = 32000;
            worksheet.Cell(2, 9).Value = 30000;
            worksheet.Cell(2, 10).Value = 100;
            worksheet.Cell(2, 11).Value = 20;
            worksheet.Cell(2, 12).Value = "Argos";
            worksheet.Cell(2, 13).Value = "Gris";
            worksheet.Cell(2, 14).Value = "Gris";
            worksheet.Cell(2, 15).Value = 50;
            worksheet.Cell(2, 16).Value = "50x30x10 cm";
            worksheet.Cell(2, 17).Value = "No";
            worksheet.Cell(2, 18).Value = "No";

            // Ajustar ancho de columnas
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}