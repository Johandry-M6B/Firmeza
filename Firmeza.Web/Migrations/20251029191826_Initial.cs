using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Firmeza.Web.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_customer = table.Column<int>(type: "integer", nullable: false),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    nit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    document_number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    credit_limit = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    days_to_pay = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    special_discount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    date_registered = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "measurements",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    abbreviation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_measurements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    trade_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    nit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    contact_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suppliers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sales",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    invoice_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    sub_total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    discount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    vat = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    payment_from = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    amount_paid = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    full_payment_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    delivery_address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    delivery_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    devoted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    observations = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sales", x => x.id);
                    table.ForeignKey(
                        name: "FK_sales_customers_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "public",
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "products",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    measurement_id = table.Column<int>(type: "integer", nullable: false),
                    supplier_id = table.Column<int>(type: "integer", nullable: true),
                    buyer_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    sale_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    wholesale_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    current_stock = table.Column<int>(type: "integer", nullable: false),
                    minimum_stock = table.Column<int>(type: "integer", nullable: false),
                    mark = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    weight = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    size = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    required_refrigeration = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    dangerous_material = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    date_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_products_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "public",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_products_measurements_measurement_id",
                        column: x => x.measurement_id,
                        principalSchema: "public",
                        principalTable: "measurements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_products_suppliers_supplier_id",
                        column: x => x.supplier_id,
                        principalSchema: "public",
                        principalTable: "suppliers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "payment_sales",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sale_id = table.Column<int>(type: "integer", nullable: false),
                    payment_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    payment_from = table.Column<int>(type: "integer", nullable: false),
                    reference_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    observations = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_sales", x => x.id);
                    table.ForeignKey(
                        name: "FK_payment_sales_sales_sale_id",
                        column: x => x.sale_id,
                        principalSchema: "public",
                        principalTable: "sales",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_movements",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    movement_type = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    after_stock = table.Column<int>(type: "integer", nullable: false),
                    new_stock = table.Column<int>(type: "integer", nullable: false),
                    observation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_movements", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventory_movements_products_product_id",
                        column: x => x.product_id,
                        principalSchema: "public",
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sales_details",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sale_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    discount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    sub_total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    vat_percentage = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 19m),
                    vat = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    observations = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sales_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_sales_details_products_product_id",
                        column: x => x.product_id,
                        principalSchema: "public",
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sales_details_sales_sale_id",
                        column: x => x.sale_id,
                        principalSchema: "public",
                        principalTable: "sales",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_categories_name",
                schema: "public",
                table: "categories",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_customers_document",
                schema: "public",
                table: "customers",
                column: "document_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customers_fullname",
                schema: "public",
                table: "customers",
                column: "full_name");

            migrationBuilder.CreateIndex(
                name: "ix_customers_type",
                schema: "public",
                table: "customers",
                column: "type_customer");

            migrationBuilder.CreateIndex(
                name: "ix_inventory_movements_date",
                schema: "public",
                table: "inventory_movements",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "ix_inventory_movements_product",
                schema: "public",
                table: "inventory_movements",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_inventory_movements_type",
                schema: "public",
                table: "inventory_movements",
                column: "movement_type");

            migrationBuilder.CreateIndex(
                name: "ix_measurements_name",
                schema: "public",
                table: "measurements",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_payment_sales_date",
                schema: "public",
                table: "payment_sales",
                column: "payment_date");

            migrationBuilder.CreateIndex(
                name: "ix_payment_sales_sale",
                schema: "public",
                table: "payment_sales",
                column: "sale_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_category",
                schema: "public",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_code",
                schema: "public",
                table: "products",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_measurement_id",
                schema: "public",
                table: "products",
                column: "measurement_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_name",
                schema: "public",
                table: "products",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_products_supplier_id",
                schema: "public",
                table: "products",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "ix_sales_customer",
                schema: "public",
                table: "sales",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_sales_date",
                schema: "public",
                table: "sales",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "ix_sales_invoice_number",
                schema: "public",
                table: "sales",
                column: "invoice_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sales_status",
                schema: "public",
                table: "sales",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_sales_details_product",
                schema: "public",
                table: "sales_details",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_sales_details_sale",
                schema: "public",
                table: "sales_details",
                column: "sale_id");

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_nit",
                schema: "public",
                table: "suppliers",
                column: "nit",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_trade_name",
                schema: "public",
                table: "suppliers",
                column: "trade_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventory_movements",
                schema: "public");

            migrationBuilder.DropTable(
                name: "payment_sales",
                schema: "public");

            migrationBuilder.DropTable(
                name: "sales_details",
                schema: "public");

            migrationBuilder.DropTable(
                name: "products",
                schema: "public");

            migrationBuilder.DropTable(
                name: "sales",
                schema: "public");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "public");

            migrationBuilder.DropTable(
                name: "measurements",
                schema: "public");

            migrationBuilder.DropTable(
                name: "suppliers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "customers",
                schema: "public");
        }
    }
}
