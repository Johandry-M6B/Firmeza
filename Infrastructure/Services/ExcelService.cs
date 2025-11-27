using ClosedXML.Excel;
using Domain.Interfaces;

namespace Infrastructure.Services;

public class ExcelService : IExcelService
{
     public async Task<IEnumerable<T>> ImportFromExcelAsync<T>(Stream fileStream) where T : class
    {
        var list = new List<T>();

        using var workbook = new XLWorkbook(fileStream);
        var worksheet = workbook.Worksheet(1);
        var properties = typeof(T).GetProperties();

        // Primera fila son los headers
        var headerRow = worksheet.FirstRowUsed();
        var headers = new Dictionary<string, int>();
    
        foreach (var cell in headerRow?.CellsUsed()!)
        {
            headers[cell.Value.ToString().Trim()] = cell.Address.ColumnNumber;
        }

        // Leer las filas de datos
        var rows = worksheet.RowsUsed().Skip(1); // Saltar header

        foreach (var row in rows)
        {
            var item = Activator.CreateInstance<T>();

            foreach (var prop in properties)
            {
                if (headers.TryGetValue(prop.Name, out int columnIndex))
                {
                    var cellValue = row.Cell(columnIndex).Value;
                    
                    if (cellValue.IsBlank && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                    {
                        var value = Convert.ChangeType(cellValue, prop.PropertyType);
                        prop.SetValue(item, value);
                    }
                }
            }

            list.Add(item);
        }

        return await Task.FromResult(list);
    }

    public async Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, string sheetName) where T : class
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        var properties = typeof(T).GetProperties();

        // Headers
        for (int i = 0; i < properties.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = properties[i].Name;
            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        // Data
        int row = 2;
        foreach (var item in data)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(item);
                worksheet.Cell(row, i + 1).Value = value?.ToString() ?? string.Empty;
            }
            row++;
        }

        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return await Task.FromResult(stream.ToArray());
    }

    public byte[] GenerateProductTemplate()
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Productos");

        // Encabezados
        worksheet.Cell(1, 1).Value = "Code";
        worksheet.Cell(1, 2).Value = "Name";
        worksheet.Cell(1, 3).Value = "Description";
        worksheet.Cell(1, 4).Value = "CategoryName";
        worksheet.Cell(1, 5).Value = "MeasurementName";
        worksheet.Cell(1, 6).Value = "SupplierName";
        worksheet.Cell(1, 7).Value = "BuyerPrice";
        worksheet.Cell(1, 8).Value = "SalePrice";
        worksheet.Cell(1, 9).Value = "WholesalePrice";
        worksheet.Cell(1, 10).Value = "CurrentStock";
        worksheet.Cell(1, 11).Value = "MinimumStock";
        worksheet.Cell(1, 12).Value = "Mark";
        worksheet.Cell(1, 13).Value = "Model";
        worksheet.Cell(1, 14).Value = "Color";
        worksheet.Cell(1, 15).Value = "Weight";
        worksheet.Cell(1, 16).Value = "Size";
        worksheet.Cell(1, 17).Value = "RequiredRefrigeration";
        worksheet.Cell(1, 18).Value = "DangerousMaterial";

        // Formato de encabezados
        var headerRange = worksheet.Range(1, 1, 1, 18);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // Ejemplo de datos (fila 2)
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