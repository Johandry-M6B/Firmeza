using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Firmeza.Web.Data.Entities;

namespace Domain.Entities;

public class PaymentSale
{
   [Key]
   public int Id { get; set; }
   [Required]
   public int SaleId { get; set; }
   [ForeignKey(nameof(SaleId))]
   public virtual Sale Sale { get; set; } = null!;
   public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
   [Column(TypeName = "decimal(18,2)")]
   public decimal Amount { get; set; }
   public PaymentFrom PaymentFrom { get; set; }
   [MaxLength(50)]
   public string? ReferenceNumber { get; set; }
   [MaxLength(200)]
   public string? Observations { get; set; }
   public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}