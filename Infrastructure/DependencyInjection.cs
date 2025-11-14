// Firmeza.Infrastructure/DependencyInjection.cs

using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Firmeza.Infrastructure.Services;
using Infrastructure.Repositories;
using Infrastructure.Services;

namespace Firmeza.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ============================================
        // DATABASE
        // ============================================
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Firmeza.Infrastructure")));

        // ============================================
        // REPOSITORIES
        // ============================================
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IMeasurementRepository, MeasurementRepository>();
        services.AddScoped<IPaymentSaleRepository, PaymentSaleRepository>();
        services.AddScoped<IInventoryMovementRepository, InventoryMovementRepository>();

        // ============================================
        // SERVICES
        // ============================================
        services.AddScoped<IExcelService, ExcelService>();
        services.AddScoped<IPdfService, PdfService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Para CurrentUserService
        services.AddHttpContextAccessor();

        return services;
    }
}