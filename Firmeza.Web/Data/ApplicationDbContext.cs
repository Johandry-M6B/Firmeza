using Firmeza.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
     // DbSets
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SalesDetail> SalesDetails { get; set; }
        public DbSet<PaymentSale> PaymentSales { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar esquema para PostgreSQL
            modelBuilder.HasDefaultSchema("public");

            // ============================================
            // CONFIGURACIÓN DE CATEGORY
            // ============================================
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
                entity.Property(e => e.Active).HasColumnName("active").HasDefaultValue(true);
                entity.Property(e => e.DateCreated).HasColumnName("date_created").HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Índices
                entity.HasIndex(e => e.Name).HasDatabaseName("ix_categories_name");
            });

            // ============================================
            // CONFIGURACIÓN DE SUPPLIER
            // ============================================
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("suppliers");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TradeName).HasColumnName("trade_name").IsRequired().HasMaxLength(20);
                entity.Property(e => e.Nit).HasColumnName("nit").IsRequired().HasMaxLength(20);
                entity.Property(e => e.ContactName).HasColumnName("contact_name").HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(15);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(100);
                entity.Property(e => e.Address).HasColumnName("address").HasMaxLength(200);
                entity.Property(e => e.City).HasColumnName("city").HasMaxLength(100);
                entity.Property(e => e.Active).HasColumnName("active").HasDefaultValue(true);
                entity.Property(e => e.DateCreated).HasColumnName("date_created").HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Índices
                entity.HasIndex(e => e.Nit).IsUnique().HasDatabaseName("ix_suppliers_nit");
                entity.HasIndex(e => e.TradeName).HasDatabaseName("ix_suppliers_trade_name");
            });

            // ============================================
            // CONFIGURACIÓN DE MEASUREMENT
            // ============================================
            modelBuilder.Entity<Measurement>(entity =>
            {
                entity.ToTable("measurements");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
                entity.Property(e => e.Abbreviation).HasColumnName("abbreviation").IsRequired().HasMaxLength(10);
                entity.Property(e => e.Active).HasColumnName("active").HasDefaultValue(true);

                // Índices
                entity.HasIndex(e => e.Name).HasDatabaseName("ix_measurements_name");
            });

            // ============================================
            // CONFIGURACIÓN DE PRODUCT
            // ============================================
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Code).HasColumnName("code").IsRequired().HasMaxLength(20);
                entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
                entity.Property(e => e.CategoryId).HasColumnName("category_id");
                entity.Property(e => e.MeasurementId).HasColumnName("measurement_id");
                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
                entity.Property(e => e.BuyerPrice).HasColumnName("buyer_price").HasColumnType("decimal(18,2)");
                entity.Property(e => e.SalePrice).HasColumnName("sale_price").HasColumnType("decimal(18,2)");
                entity.Property(e => e.WholesalePrice).HasColumnName("wholesale_price").HasColumnType("decimal(18,2)");
                entity.Property(e => e.CurrentStock).HasColumnName("current_stock");
                entity.Property(e => e.MinimumStock).HasColumnName("minimum_stock");
                entity.Property(e => e.Mark).HasColumnName("mark").HasMaxLength(50);
                entity.Property(e => e.Model).HasColumnName("model").HasMaxLength(100);
                entity.Property(e => e.Color).HasColumnName("color").HasMaxLength(50);
                entity.Property(e => e.Weight).HasColumnName("weight").HasColumnType("decimal(18,2)");
                entity.Property(e => e.Size).HasColumnName("size").HasMaxLength(50);
                entity.Property(e => e.RequiredRefrigeration).HasColumnName("required_refrigeration").HasDefaultValue(false);
                entity.Property(e => e.DangerousMaterial).HasColumnName("dangerous_material").HasDefaultValue(false);
                entity.Property(e => e.Active).HasColumnName("active").HasDefaultValue(true);
                entity.Property(e => e.DateCreated).HasColumnName("date_created").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.DateUpdated).HasColumnName("date_updated");

                // Relaciones
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Measurement)
                    .WithMany(m => m.Products)
                    .HasForeignKey(e => e.MeasurementId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Supplier)
                    .WithMany(s => s.Products)
                    .HasForeignKey(e => e.SupplierId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Índices
                entity.HasIndex(e => e.Code).IsUnique().HasDatabaseName("ix_products_code");
                entity.HasIndex(e => e.Name).HasDatabaseName("ix_products_name");
                entity.HasIndex(e => e.CategoryId).HasDatabaseName("ix_products_category");
            });

            // ============================================
            // CONFIGURACIÓN DE CUSTOMER
            // ============================================
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customers");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TypeCustomer).HasColumnName("type_customer").HasConversion<int>();
                entity.Property(e => e.FullName).HasColumnName("full_name").IsRequired().HasMaxLength(200);
                entity.Property(e => e.Nit).HasColumnName("nit").HasMaxLength(20);
                entity.Property(e => e.DocumentNumber).HasColumnName("document_number").IsRequired().HasMaxLength(15);
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(15);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(100);
                entity.Property(e => e.City).HasColumnName("city").HasMaxLength(100);
                entity.Property(e => e.Country).HasColumnName("country").HasMaxLength(100);
                entity.Property(e => e.CreditLimit).HasColumnName("credit_limit").HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.DaysToPay).HasColumnName("days_to_pay").HasDefaultValue(0);
                entity.Property(e => e.SpecialDiscount).HasColumnName("special_discount").HasColumnType("decimal(18,2)");
                entity.Property(e => e.Active).HasColumnName("active").HasDefaultValue(true);
                entity.Property(e => e.DateRegistered).HasColumnName("date_registered").HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Índices
                entity.HasIndex(e => e.DocumentNumber).IsUnique().HasDatabaseName("ix_customers_document");
                entity.HasIndex(e => e.FullName).HasDatabaseName("ix_customers_fullname");
                entity.HasIndex(e => e.TypeCustomer).HasDatabaseName("ix_customers_type");
            });

            // ============================================
            // CONFIGURACIÓN DE SALE
            // ============================================
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.ToTable("sales");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.InvoiceNumber).HasColumnName("invoice_number").IsRequired().HasMaxLength(20);
                entity.Property(e => e.Date).HasColumnName("date").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.CustomerId).HasColumnName("customer_id");
                entity.Property(e => e.SubTotal).HasColumnName("sub_total").HasColumnType("decimal(18,2)");
                entity.Property(e => e.Discount).HasColumnName("discount").HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.Vat).HasColumnName("vat").HasColumnType("decimal(18,2)");
                entity.Property(e => e.Total).HasColumnName("total").HasColumnType("decimal(18,2)");
                entity.Property(e => e.PaymentFrom).HasColumnName("payment_from").HasConversion<int>();
                entity.Property(e => e.Status).HasColumnName("status").HasConversion<int>();
                entity.Property(e => e.AmountPaid).HasColumnName("amount_paid").HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.Balance).HasColumnName("balance").HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.FullPaymentDate).HasColumnName("full_payment_date");
                entity.Property(e => e.DeliveryAddress).HasColumnName("delivery_address").HasMaxLength(200);
                entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
                entity.Property(e => e.Devoted).HasColumnName("devoted").HasDefaultValue(false);
                entity.Property(e => e.Observations).HasColumnName("observations").HasMaxLength(500);
                entity.Property(e => e.DateCreated).HasColumnName("date_created").HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Relaciones
                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Sales)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Índices
                entity.HasIndex(e => e.InvoiceNumber).IsUnique().HasDatabaseName("ix_sales_invoice_number");
                entity.HasIndex(e => e.Date).HasDatabaseName("ix_sales_date");
                entity.HasIndex(e => e.CustomerId).HasDatabaseName("ix_sales_customer");
                entity.HasIndex(e => e.Status).HasDatabaseName("ix_sales_status");
            });

            // ============================================
            // CONFIGURACIÓN DE SALES DETAIL
            // ============================================
            modelBuilder.Entity<SalesDetail>(entity =>
            {
                entity.ToTable("sales_details");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.SaleId).HasColumnName("sale_id");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.Price).HasColumnName("price").HasColumnType("decimal(18,2)");
                entity.Property(e => e.Discount).HasColumnName("discount").HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.SubTotal).HasColumnName("sub_total").HasColumnType("decimal(18,2)");
                entity.Property(e => e.VatPercentage).HasColumnName("vat_percentage").HasColumnType("decimal(18,2)").HasDefaultValue(19);
                entity.Property(e => e.Vat).HasColumnName("vat").HasColumnType("decimal(18,2)");
                entity.Property(e => e.Total).HasColumnName("total").HasColumnType("decimal(18,2)");
                entity.Property(e => e.Observations).HasColumnName("observations").HasMaxLength(200);

                // Relaciones
                entity.HasOne(e => e.Sale)
                    .WithMany(s => s.SalesDetails)
                    .HasForeignKey(e => e.SaleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                    .WithMany(p => p.SalesDetails)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Índices
                entity.HasIndex(e => e.SaleId).HasDatabaseName("ix_sales_details_sale");
                entity.HasIndex(e => e.ProductId).HasDatabaseName("ix_sales_details_product");
            });

            // ============================================
            // CONFIGURACIÓN DE PAYMENT SALE
            // ============================================
            modelBuilder.Entity<PaymentSale>(entity =>
            {
                entity.ToTable("payment_sales");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.SaleId).HasColumnName("sale_id");
                entity.Property(e => e.PaymentDate).HasColumnName("payment_date").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.Amount).HasColumnName("amount").HasColumnType("decimal(18,2)");
                entity.Property(e => e.PaymentFrom).HasColumnName("payment_from").HasConversion<int>();
                entity.Property(e => e.ReferenceNumber).HasColumnName("reference_number").HasMaxLength(50);
                entity.Property(e => e.Observations).HasColumnName("observations").HasMaxLength(200);
                entity.Property(e => e.DateCreated).HasColumnName("date_created").HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Relaciones
                entity.HasOne(e => e.Sale)
                    .WithMany(s => s.PaymentSales)
                    .HasForeignKey(e => e.SaleId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Índices
                entity.HasIndex(e => e.SaleId).HasDatabaseName("ix_payment_sales_sale");
                entity.HasIndex(e => e.PaymentDate).HasDatabaseName("ix_payment_sales_date");
            });

            // ============================================
            // CONFIGURACIÓN DE INVENTORY MOVEMENT
            // ============================================
            modelBuilder.Entity<InventoryMovement>(entity =>
            {
                entity.ToTable("inventory_movements");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.MovementType).HasColumnName("movement_type").HasConversion<int>();
                entity.Property(e => e.Date).HasColumnName("date").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.AfterStock).HasColumnName("after_stock");
                entity.Property(e => e.NewStock).HasColumnName("new_stock");
                entity.Property(e => e.Observation).HasColumnName("observation").HasMaxLength(200);
                entity.Property(e => e.DateCreated).HasColumnName("date_created").HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Relaciones
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.InventoryMovements)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Índices
                entity.HasIndex(e => e.ProductId).HasDatabaseName("ix_inventory_movements_product");
                entity.HasIndex(e => e.Date).HasDatabaseName("ix_inventory_movements_date");
                entity.HasIndex(e => e.MovementType).HasDatabaseName("ix_inventory_movements_type");
            });
        }
    }
