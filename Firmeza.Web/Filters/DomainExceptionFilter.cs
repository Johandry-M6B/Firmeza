using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Firmeza.Web.Filters;

public class DomainExceptionFilter : IExceptionFilter
{
    private readonly ILogger<DomainExceptionFilter> _logger;

    public DomainExceptionFilter(ILogger<DomainExceptionFilter> logger)
    {
        _logger = logger;
    }


    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DomainException domainException)
        {
            _logger.LogWarning(
                domainException,
                "Domain exception occurred: {ErrorCode} - {Message}",
                domainException.ErrorCode,
                domainException.Message);
            
            var statusCode = domainException switch
            {
                ProductNotFoundException => 404,
                DuplicateProductCodeException => 409, // Conflict
                InvalidPriceException => 400,
                InsufficientStockException => 400,
                InvalidDiscountException => 400,
                InvalidQuantityException => 400,
                SaleAlreadyPaidException => 409,
                CreditLimitExceededException => 400,
                _ => 400 // Bad Request por defecto
            };

            var response = new
            {
                error = new
                {
                    code = domainException.ErrorCode,
                    message = domainException.Message,
                    details = domainException.ErrorDetails,
                    timestamp = DateTime.UtcNow
                }
            };
            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };
            context.ExceptionHandled = true;
        }
    }
}