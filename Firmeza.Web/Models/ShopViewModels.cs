using System.ComponentModel.DataAnnotations;
using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Models.ViewModels
{
    // ViewModel para la página principal de la tienda
    public class ShopIndexViewModel
    {
        public List<Product> Products { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public string? SearchTerm { get; set; }
        public int? SelectedCategoryId { get; set; }
    }

    // ViewModel del carrito de compras
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();

        public decimal Subtotal => Items.Sum(i => i.Subtotal);
        public decimal Tax => Subtotal * 0.19m; // IVA 19%
        public decimal Total => Subtotal + Tax;
        public int TotalItems => Items.Sum(i => i.Quantity);
    }

    // ViewModel de un item del carrito
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string MeasurementName { get; set; } = string.Empty;
        public int MaxStock { get; set; }

        public decimal Subtotal => Price * Quantity;
        public decimal Tax => Subtotal * 0.19m;
        public decimal Total => Subtotal + Tax;
    }

    // ViewModel para el checkout
    public class CheckoutViewModel
    {
        public CartViewModel Cart { get; set; } = new();

        [Required(ErrorMessage = "El nombre completo es requerido")]
        [Display(Name = "Nombre Completo")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de documento es requerido")]
        [Display(Name = "Número de Documento")]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "Número de teléfono inválido")]
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ciudad es requerida")]
        [Display(Name = "Ciudad")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección de entrega es requerida")]
        [Display(Name = "Dirección de Entrega")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seleccione un método de pago")]
        [Display(Name = "Método de Pago")]
        public PaymentFrom PaymentMethod { get; set; }

        [Display(Name = "Observaciones")]
        public string? Observations { get; set; }
    }
}