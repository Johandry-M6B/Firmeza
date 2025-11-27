namespace Domain.Exceptions;

public class CreditLimitExceededException : DomainException
{
    public int CustmoerId { get; }
    public string CustomerName { get; }
    public decimal CreditLimit { get; }
    public decimal CurrentDebt { get; }
    public decimal NewSaleAmount { get; }

    public CreditLimitExceededException(
        int customerId,
        string customerName,
        decimal creditLimit,
        decimal currentDebt,
        decimal newSaleAmount)
        : base(
            $"The Customer '{customerName}' would exceed your credit limit." +
            $"Limit: ${creditLimit:NO}, Current Debt: ${currentDebt:NO}," +
            $"New Sale: ${newSaleAmount:NO}, Total: ${(currentDebt + newSaleAmount):N0}",
            "CREDIT_LIMIT_EXCEEDED")
    {
        CustmoerId = customerId;
        CustomerName = customerName;
        CreditLimit = creditLimit;
        CurrentDebt = currentDebt;
        NewSaleAmount = newSaleAmount;
        
        AddErrorDetail("CustomerId", customerId);
        AddErrorDetail("CustomerName", customerName);
        AddErrorDetail("CreditLimit", creditLimit);
        AddErrorDetail("CurrentDebt", currentDebt);
        AddErrorDetail("NewSaleAmount", newSaleAmount);
        AddErrorDetail("TotalDebt", currentDebt + newSaleAmount);
        AddErrorDetail("ExceededAmount", (currentDebt + newSaleAmount) - creditLimit);
    }
}
