using Application;
using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Filters;
using Infrastructure;
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
// IDENTITY
// ============================================

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<Infrastructure.Persistence.ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

// Configurar cookies de autenticación
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
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
    try
    {
        // Ejecutar migraciones pendientes
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        // Seed de roles y usuario admin
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        await IdentitySeeder.SeedAsync(userManager, roleManager);
        
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Base de datos inicializada correctamente");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al inicializar la base de datos");
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
    pattern: "{controller=Shop}/{action=Index}/{id?}");

app.Run();