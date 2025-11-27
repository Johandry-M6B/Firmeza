
using Domain.Enums;

namespace Domain.Entities;

public class PaymentSale
{
   
   public int Id { get; set; }
   
   public int SaleId { get; set; }
   public Sale Sale { get; set; } = null!;
   public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
   
   public decimal Amount { get; set; }
   public PaymentFrom PaymentFrom { get; set; }
   
   public string? ReferenceNumber { get; set; }
   
   public string? Observations { get; set; }
   public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}