using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Controllers
{
    [AllowAnonymous] // ⬅️ Permite acceso sin login
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "ShoppingCart";

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Shop
        public async Task<IActionResult> Index(string searchTerm, int? categoryId)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Measurement)
                .Where(p => p.Active && p.CurrentStock > 0);

            // Filtrar por búsqueda
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => 
                    p.Name.Contains(searchTerm) || 
                    p.Description.Contains(searchTerm) ||
                    p.Code.Contains(searchTerm));
            }

            // Filtrar por categoría
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            var products = await query.OrderBy(p => p.Name).ToListAsync();
            var categories = await _context.Categories
                .Where(c => c.Active)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var viewModel = new ShopIndexViewModel
            {
                Products = products,
                Categories = categories,
                SearchTerm = searchTerm,
                SelectedCategoryId = categoryId
            };

            return View(viewModel);
        }

        // GET: /Shop/Product/5
        public async Task<IActionResult> Product(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Measurement)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id && p.Active);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: /Shop/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _context.Products
                .Include(p => p.Measurement)
                .FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Producto no encontrado" });
            }

            if (quantity > product.CurrentStock)
            {
                return Json(new { success = false, message = "Stock insuficiente" });
            }

            // Obtener carrito de la sesión
            var cart = GetCart();

            // Verificar si el producto ya está en el carrito
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                if (existingItem.Quantity > product.CurrentStock)
                {
                    existingItem.Quantity = product.CurrentStock;
                }
            }
            else
            {
                cart.Items.Add(new CartItemViewModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.SalePrice,
                    Quantity = quantity,
                    MeasurementName = product.Measurement.Abbreviation,
                    MaxStock = product.CurrentStock
                });
            }

            SaveCart(cart);

            return Json(new { 
                success = true, 
                message = "Producto agregado al carrito",
                cartCount = cart.Items.Sum(i => i.Quantity)
            });
        }

        // GET: /Shop/Cart
        public IActionResult Cart()
        {
            var cart = GetCart();
            return View(cart);
        }

        // POST: /Shop/UpdateCartItem
        [HttpPost]
        public IActionResult UpdateCartItem(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Items.Remove(item);
                }
                else if (quantity <= item.MaxStock)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    return Json(new { success = false, message = "Stock insuficiente" });
                }

                SaveCart(cart);
            }

            return Json(new { success = true, total = cart.Total });
        }

        // POST: /Shop/RemoveFromCart
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                cart.Items.Remove(item);
                SaveCart(cart);
            }

            return Json(new { success = true, cartCount = cart.Items.Count });
        }

        // GET: /Shop/Checkout
        public IActionResult Checkout()
        {
            var cart = GetCart();

            if (!cart.Items.Any())
            {
                TempData["ErrorMessage"] = "El carrito está vacío";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new CheckoutViewModel
            {
                Cart = cart
            };

            return View(viewModel);
        }

        // POST: /Shop/PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Cart = GetCart();
                return View("Checkout", model);
            }

            var cart = GetCart();

            if (!cart.Items.Any())
            {
                TempData["ErrorMessage"] = "El carrito está vacío";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Crear o buscar cliente
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.DocumentNumber == model.DocumentNumber);

                if (customer == null)
                {
                    customer = new Customer
                    {
                        FullName = model.FullName,
                        DocumentNumber = model.DocumentNumber,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        City = model.City,
                        TypeCustomer = TypeCustomer.Retail,
                        Active = true
                    };
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                }

                // Generar número de factura
                var lastSale = await _context.Sales
                    .OrderByDescending(s => s.Id)
                    .FirstOrDefaultAsync();
                
                var nextNumber = (lastSale?.Id ?? 0) + 1;
                var invoiceNumber = $"FACT-{nextNumber:D6}";

                // Crear venta
                var sale = new Sale
                {
                    InvoiceNumber = invoiceNumber,
                    Date = DateTime.UtcNow,
                    CustomerId = customer.Id,
                    SubTotal = cart.Subtotal,
                    Discount = 0,
                    Vat = cart.Tax,
                    Total = cart.Total,
                    PaymentFrom = model.PaymentMethod,
                    Status = model.PaymentMethod == PaymentFrom.Credit 
                        ? SaleStatus.Credit 
                        : SaleStatus.Pending,
                    AmountPaid = 0,
                    Balance = cart.Total,
                    DeliveryAddress = model.DeliveryAddress,
                    Observations = model.Observations
                };

                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                // Crear detalles de venta
                foreach (var item in cart.Items)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);

                    if (product == null || product.CurrentStock < item.Quantity)
                    {
                        throw new Exception($"Stock insuficiente para {item.ProductName}");
                    }

                    var detail = new SalesDetail
                    {
                        SaleId = sale.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Discount = 0,
                        SubTotal = item.Subtotal,
                        VatPercentage = 19,
                        Vat = item.Tax,
                        Total = item.Total
                    };

                    _context.SalesDetails.Add(detail);

                    // Actualizar stock
                    product.CurrentStock -= item.Quantity;

                    // Registrar movimiento de inventario
                    var movement = new InventoryMovement
                    {
                        ProductId = product.Id,
                        MovementType = MovementType.Output,
                        Date = DateTime.UtcNow,
                        Quantity = item.Quantity,
                        AfterStock = product.CurrentStock + item.Quantity,
                        NewStock = product.CurrentStock,
                        Observation = $"Venta Online - Factura {invoiceNumber}"
                    };

                    _context.InventoryMovements.Add(movement);
                }

                await _context.SaveChangesAsync();

                // Limpiar carrito
                ClearCart();

                TempData["SuccessMessage"] = "¡Pedido realizado exitosamente!";
                return RedirectToAction("OrderConfirmation", new { id = sale.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al procesar el pedido: {ex.Message}");
                model.Cart = GetCart();
                return View("Checkout", model);
            }
        }

        // GET: /Shop/OrderConfirmation/5
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SalesDetails)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // Métodos auxiliares para el carrito
        private CartViewModel GetCart()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            
            if (string.IsNullOrEmpty(cartJson))
            {
                return new CartViewModel();
            }

            return System.Text.Json.JsonSerializer.Deserialize<CartViewModel>(cartJson) 
                   ?? new CartViewModel();
        }

        private void SaveCart(CartViewModel cart)
        {
            var cartJson = System.Text.Json.JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartSessionKey, cartJson);
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove(CartSessionKey);
        }
    }
}