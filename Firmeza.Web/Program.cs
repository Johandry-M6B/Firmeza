using Application;
using Domain.Enums;
using Firmeza.Web.Data;
using Firmeza.Web.Filters;
using Infrastructure;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ApplicationDbContext = Infrastructure.Persistence.ApplicationDbContext;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// SERVICIOS DE CLEAN ARCHITECTURE
// ============================================

// Registrar Application (MediatR, AutoMapper, Validators)
builder.Services.AddApplication();

// Registrar Infrastructure (Repositorios, DbContext, Servicios)
builder.Services.AddInfrastructure(builder.Configuration);

// ============================================
// SERVICIOS DE MVC Y RAZOR PAGES
// ============================================

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<DomainExceptionFilter>();
});

// ============================================
// SESIÓN Y CACHE
// ============================================

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ============================================
// AUTORIZACIÓN
// ============================================

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy => 
        policy.RequireRole(UserRoles.Admin));
});

// ============================================
// BUILD APP
// ============================================

var app = builder.Build();

// ============================================
// SEED DE DATOS INICIALES
// ============================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        // Ejecutar migraciones pendientes
        var context = services.GetRequiredService<ApplicationDbContext>();
        logger.LogInformation("Base de datos inicializada correctamente");
        await context.Database.MigrateAsync();
        
        // Obtener UserManager y RoleManager
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        
        logger.LogInformation("Sembrando datos iniciales...");
        await ApplicationDbContextSeed.SeedAsync(context, userManager);
        logger.LogInformation("Datos iniciales sembrados correctamente.");
        
        logger.LogInformation("Sembrando roles y usuario admin...");
        await IdentitySeeder.SeedAsync(userManager, roleManager);
        logger.LogInformation("Datos iniciales roles y usuario admin...");
        
        
        logger.LogInformation("Base de datos inicializada correctamente.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al inicializar la base de datos");
        if (app.Environment.IsDevelopment())
        {
            throw;
        }
        
    }
}

// ============================================
// CONFIGURAR MIDDLEWARE PIPELINE
// ============================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();