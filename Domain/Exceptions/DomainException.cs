namespace Domain.Exceptions;

public class DomainException : Exception
{
   public string ErrorCode { get; }
   
   public Dictionary<string, object> ErrorDetails { get; }

   public DomainException(string message, string errorCode)
       : base(message)
   {
        ErrorCode = errorCode;
        ErrorDetails = new Dictionary<string, object>();
   }

   protected DomainException(string message, string errorCode, Exception innerException)
       : base(message, innerException)
   {
       ErrorCode = errorCode;
         ErrorDetails = new Dictionary<string, object>();
   }
   
    public void AddErrorDetail(string key, object value)
    {
         ErrorDetails[key] = value;
    }
}   