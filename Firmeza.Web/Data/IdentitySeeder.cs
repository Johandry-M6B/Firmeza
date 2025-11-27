using Domain.Enums;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Firmeza.Web.Data;

public static class IdentitySeeder
{
    // Definir roles aquí por si acaso
    private const string AdminRole = "Admin";
    private const string EmployeeRole = "Employee";
    private const string CustomerRole = "Customer";

    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        Console.WriteLine("🌱 Iniciando seed de Identity...");
        
        // 1. Crear todos los roles
        var roles = new[] { AdminRole, EmployeeRole, CustomerRole };
        
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                Console.WriteLine($"🔨 Creando rol '{roleName}'...");
                
                var role = new IdentityRole(roleName);
                var result = await roleManager.CreateAsync(role);
                
                if (result.Succeeded)
                {
                    Console.WriteLine($"✅ Rol '{roleName}' creado exitosamente");
                }
                else
                {
                    Console.WriteLine($"❌ ERROR creando rol '{roleName}':");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"   - {error.Code}: {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"ℹ️  Rol '{roleName}' ya existe");
            }
        }

        // Verificar que los roles se crearon
        var allRoles = roleManager.Roles.ToList();
        Console.WriteLine($"📊 Total de roles en la BD: {allRoles.Count}");
        foreach (var role in allRoles)
        {
            Console.WriteLine($"   - {role.Name} (ID: {role.Id})");
        }

        // 2. Crear usuario admin
        var adminEmail = "admin@firmeza.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            Console.WriteLine("🔨 Creando usuario administrador...");
            
            adminUser = new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "System",
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                IsActive = true,
                PhoneNumber = "3238817902",
                DocumentNumber = "1001877889"
            };

            var createResult = await userManager.CreateAsync(adminUser, "Admin123*");
            
            if (createResult.Succeeded)
            {
                Console.WriteLine($"✅ Usuario '{adminEmail}' creado");
                
                var addRoleResult = await userManager.AddToRoleAsync(adminUser, AdminRole);
                
                if (addRoleResult.Succeeded)
                {
                    Console.WriteLine($"✅ Rol '{AdminRole}' asignado al usuario");
                    Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    Console.WriteLine("📧 Email: admin@firmeza.com");
                    Console.WriteLine("🔑 Password: Admin123*");
                    Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                }
                else
                {
                    Console.WriteLine($"❌ ERROR asignando rol '{AdminRole}':");
                    foreach (var error in addRoleResult.Errors)
                    {
                        Console.WriteLine($"   - {error.Code}: {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("❌ ERROR creando usuario administrador:");
                foreach (var error in createResult.Errors)
                {
                    Console.WriteLine($"   - {error.Code}: {error.Description}");
                }
            }
        }
        else
        {
            Console.WriteLine($"ℹ️  Usuario '{adminEmail}' ya existe");
            
            // Verificar roles del usuario
            var userRoles = await userManager.GetRolesAsync(adminUser);
            Console.WriteLine($"   Roles actuales: {string.Join(", ", userRoles)}");
            
            // Asegurar que tiene el rol Admin
            if (!await userManager.IsInRoleAsync(adminUser, AdminRole))
            {
                var addRoleResult = await userManager.AddToRoleAsync(adminUser, AdminRole);
                if (addRoleResult.Succeeded)
                {
                    Console.WriteLine($"✅ Rol '{AdminRole}' asignado a {adminEmail}");
                }
            }
            
            // Asegurar que está activo
            if (!adminUser.IsActive || !adminUser.EmailConfirmed)
            {
                adminUser.IsActive = true;
                adminUser.EmailConfirmed = true;
                await userManager.UpdateAsync(adminUser);
                Console.WriteLine($"✅ Usuario {adminEmail} actualizado (activo y confirmado)");
            }
        }
        
        Console.WriteLine("✅ Seed de Identity completado");
        Console.WriteLine("════════════════════════════════════════");
    }
}