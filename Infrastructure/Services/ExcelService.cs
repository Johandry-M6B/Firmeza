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
}