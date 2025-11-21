namespace Domain.Interfaces;

public interface IExcelService
{
    Task<IEnumerable<T>> ImportFromExcelAsync<T>(Stream fileStream) where T : class;
    Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, string sheetName) where T : class;
    byte[] GenerateProductTemplate();
    
    
}