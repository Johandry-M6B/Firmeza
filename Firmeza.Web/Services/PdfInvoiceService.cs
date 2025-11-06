using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Services;
    public interface IPdfInvoiceService
    {
        Task<byte[]> GenerateInvoicePdf(int saleId);
    }

    public class PdfInvoiceService : IPdfInvoiceService
    {
        private readonly ApplicationDbContext _context;

        public PdfInvoiceService(ApplicationDbContext context)
        {
            _context = context;
            
            // Configurar licencia (Community - gratis)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GenerateInvoicePdf(int saleId)
        {
            // Obtener datos de la venta
            var sale = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SalesDetails)
                    .ThenInclude(d => d.Product)
                        .ThenInclude(p => p.Measurement)
                .FirstOrDefaultAsync(s => s.Id == saleId);

            if (sale == null)
                throw new Exception("Venta no encontrada");

            // Generar PDF
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(content => ComposeContent(content, sale));
                    page.Footer().Element(ComposeFooter);
                });
            });

            return document.GeneratePdf();
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                // Logo y nombre de la empresa (izquierda)
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("FIRMEZA").FontSize(24).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Text("Materiales de Construcción").FontSize(12).FontColor(Colors.Grey.Darken1);
                    column.Item().PaddingTop(5).Text("NIT: 900.123.456-7").FontSize(9);
                    column.Item().Text("Dirección: Calle 123 #45-67").FontSize(9);
                    column.Item().Text("Teléfono: (605) 300-1234").FontSize(9);
                    column.Item().Text("Email: ventas@firmeza.com").FontSize(9);
                });

                // Información de la factura (derecha)
                row.RelativeItem().Column(column =>
                {
                    column.Item().AlignRight().Text("FACTURA DE VENTA").FontSize(16).Bold();
                    column.Item().AlignRight().Text(text =>
                    {
                        text.Span("N° ").FontSize(10);
                        text.Span("FACT-001").FontSize(14).Bold().FontColor(Colors.Red.Darken1);
                    });
                });
            });
        }

        void ComposeContent(IContainer container, Sale sale)
        {
            container.PaddingVertical(20).Column(column =>
            {
                column.Spacing(10);

                // Información de la venta
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                           .PaddingBottom(5).Text("INFORMACIÓN DE LA VENTA").Bold();
                        
                        col.Item().PaddingTop(5).Text(text =>
                        {
                            text.Span("Fecha: ").Bold();
                            text.Span(sale.Date.ToString("dd/MM/yyyy HH:mm"));
                        });
                        
                        col.Item().Text(text =>
                        {
                            text.Span("Factura: ").Bold();
                            text.Span(sale.InvoiceNumber);
                        });
                        
                        col.Item().Text(text =>
                        {
                            text.Span("Estado: ").Bold();
                            text.Span(GetStatusText(sale.Status)).FontColor(GetStatusColor(sale.Status));
                        });
                        
                        col.Item().Text(text =>
                        {
                            text.Span("Forma de Pago: ").Bold();
                            text.Span(GetPaymentMethodText(sale.PaymentFrom));
                        });
                    });

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                           .PaddingBottom(5).Text("DATOS DEL CLIENTE").Bold();
                        
                        col.Item().PaddingTop(5).Text(text =>
                        {
                            text.Span("Cliente: ").Bold();
                            text.Span(sale.Customer.FullName);
                        });
                        
                        col.Item().Text(text =>
                        {
                            text.Span("Documento: ").Bold();
                            text.Span(sale.Customer.DocumentNumber);
                        });
                        
                        if (!string.IsNullOrEmpty(sale.Customer.PhoneNumber))
                        {
                            col.Item().Text(text =>
                            {
                                text.Span("Teléfono: ").Bold();
                                text.Span(sale.Customer.PhoneNumber);
                            });
                        }
                        
                        if (!string.IsNullOrEmpty(sale.Customer.Email))
                        {
                            col.Item().Text(text =>
                            {
                                text.Span("Email: ").Bold();
                                text.Span(sale.Customer.Email);
                            });
                        }
                    });
                });

                // Tabla de productos
                column.Item().PaddingTop(20).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(40);  // #
                        columns.RelativeColumn(3);   // Producto
                        columns.RelativeColumn(1.5f); // Medida
                        columns.RelativeColumn(1);   // Cant
                        columns.RelativeColumn(1.5f); // P. Unit
                        columns.RelativeColumn(1);   // Desc
                        columns.RelativeColumn(1.5f); // Subtotal
                        columns.RelativeColumn(1);   // IVA
                        columns.RelativeColumn(2);   // Total
                    });

                    // Encabezados
                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                              .Text("#").FontColor(Colors.White).Bold();
                        
                        header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                              .Text("Producto").FontColor(Colors.White).Bold();
                        
                        header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                              .Text("Unidad").FontColor(Colors.White).Bold();
                        
                        header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                              .AlignRight().Text("Cant.").FontColor(Colors.White).Bold();
                        
                        header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                              .AlignRight().Text("P. Unit").FontColor(Colors.White).Bold();
                        
                        header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                              .AlignRight().Text("Desc%").FontColor(Colors.White).Bold();
                        
                        header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                              .AlignRight().Text("Subtotal").FontColor(Colors.White).Bold();
                        
                        header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                              .AlignRight().Text("IVA").FontColor(Colors.White).Bold();
                        
                        header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                              .AlignRight().Text("Total").FontColor(Colors.White).Bold();
                    });

                    // Filas de productos
                    var index = 1;
                    foreach (var detail in sale.SalesDetails)
                    {
                        var bgColor = index % 2 == 0 ? Colors.Grey.Lighten3 : Colors.White;

                        table.Cell().Background(bgColor).Padding(5)
                             .Text(index.ToString());
                        
                        table.Cell().Background(bgColor).Padding(5)
                             .Text(detail.Product.Name);
                        
                        table.Cell().Background(bgColor).Padding(5)
                             .Text(detail.Product.Measurement.Abbreviation);
                        
                        table.Cell().Background(bgColor).Padding(5)
                             .AlignRight().Text(detail.Quantity.ToString());
                        
                        table.Cell().Background(bgColor).Padding(5)
                             .AlignRight().Text($"${detail.Price:N0}");
                        
                        table.Cell().Background(bgColor).Padding(5)
                             .AlignRight().Text($"{detail.Discount:N1}%");
                        
                        table.Cell().Background(bgColor).Padding(5)
                             .AlignRight().Text($"${detail.SubTotal:N0}");
                        
                        table.Cell().Background(bgColor).Padding(5)
                             .AlignRight().Text($"${detail.Vat:N0}");
                        
                        table.Cell().Background(bgColor).Padding(5)
                             .AlignRight().Text($"${detail.Total:N0}").Bold();

                        index++;
                    }
                });

                // Totales
                column.Item().PaddingTop(10).AlignRight().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Subtotal:").Bold();
                        row.ConstantItem(120).AlignRight().Text($"${sale.SubTotal:N0}");
                    });

                    if (sale.Discount > 0)
                    {
                        col.Item().Row(row =>
                        {
                            row.ConstantItem(150).Text("Descuento:").Bold();
                            row.ConstantItem(120).AlignRight().Text($"-${sale.Discount:N0}")
                               .FontColor(Colors.Red.Medium);
                        });
                    }

                    col.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("IVA (19%):").Bold();
                        row.ConstantItem(120).AlignRight().Text($"${sale.Vat:N0}");
                    });

                    col.Item().PaddingTop(5).BorderTop(2).BorderColor(Colors.Blue.Darken2)
                       .Row(row =>
                       {
                           row.ConstantItem(150).Text("TOTAL:").Bold().FontSize(14);
                           row.ConstantItem(120).AlignRight().Text($"${sale.Total:N0}")
                              .Bold().FontSize(14).FontColor(Colors.Blue.Darken2);
                       });

                    // Información de pago
                    if (sale.Status == SaleStatus.Paid || sale.Status == SaleStatus.PartiallyPaid)
                    {
                        col.Item().PaddingTop(10).Row(row =>
                        {
                            row.ConstantItem(150).Text("Monto Pagado:").Bold();
                            row.ConstantItem(120).AlignRight().Text($"${sale.AmountPaid:N0}")
                               .FontColor(Colors.Green.Darken1);
                        });

                        if (sale.Balance > 0)
                        {
                            col.Item().Row(row =>
                            {
                                row.ConstantItem(150).Text("Saldo Pendiente:").Bold();
                                row.ConstantItem(120).AlignRight().Text($"${sale.Balance:N0}")
                                   .FontColor(Colors.Red.Medium);
                            });
                        }
                    }
                });

                // Observaciones
                if (!string.IsNullOrEmpty(sale.Observations))
                {
                    column.Item().PaddingTop(20).Column(col =>
                    {
                        col.Item().Text("Observaciones:").Bold();
                        col.Item().PaddingTop(5).Border(1).BorderColor(Colors.Grey.Lighten2)
                           .Padding(10).Text(sale.Observations);
                    });
                }

                // Información de entrega
                if (!string.IsNullOrEmpty(sale.DeliveryAddress))
                {
                    column.Item().PaddingTop(10).Column(col =>
                    {
                        col.Item().Text("Información de Entrega:").Bold();
                        col.Item().Text(text =>
                        {
                            text.Span("Dirección: ").Bold();
                            text.Span(sale.DeliveryAddress);
                        });
                        if (sale.DeliveryDate.HasValue)
                        {
                            col.Item().Text(text =>
                            {
                                text.Span("Fecha: ").Bold();
                                text.Span(sale.DeliveryDate.Value.ToString("dd/MM/yyyy"));
                            });
                        }
                    });
                }
            });
        }

        void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.Span("Página ").FontSize(9);
                text.CurrentPageNumber().FontSize(9);
                text.Span(" de ").FontSize(9);
                text.TotalPages().FontSize(9);
                text.Span(" | Generado el: ").FontSize(9);
                text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(9);
            });
        }

        private string GetStatusText(SaleStatus status)
        {
            return status switch
            {
                SaleStatus.Pending => "Pendiente",
                SaleStatus.Paid => "Pagada",
                SaleStatus.Cancelled => "Anulada",
                SaleStatus.Credit => "Crédito",
                SaleStatus.PartiallyPaid => "Pago Parcial",
                _ => "Desconocido"
            };
        }

        private string GetStatusColor(SaleStatus status)
        {
            return status switch
            {
                SaleStatus.Pending => Colors.Orange.Medium,
                SaleStatus.Paid => Colors.Green.Darken1,
                SaleStatus.Cancelled => Colors.Red.Medium,
                SaleStatus.Credit => Colors.Blue.Medium,
                SaleStatus.PartiallyPaid => Colors.Yellow.Darken2,
                _ => Colors.Grey.Medium
            };
        }

        private string GetPaymentMethodText(PaymentFrom paymentFrom)
        {
            return paymentFrom switch
            {
                PaymentFrom.Cash => "Efectivo",
                PaymentFrom.Transfer => "Transferencia",
                PaymentFrom.CreditCard => "Tarjeta de Crédito",
                PaymentFrom.DebitCard => "Tarjeta de Débito",
                PaymentFrom.Check => "Cheque",
                PaymentFrom.Credit => "Crédito",
                _ => "No especificado"
            };
        }
    }
