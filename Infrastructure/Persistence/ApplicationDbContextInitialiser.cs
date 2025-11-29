using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationDbContextInitialiser(
        ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            Console.WriteLine($"DEBUG: IsNpgsql: {_context.Database.IsNpgsql()}");
            if (_context.Database.IsNpgsql())
            {
                Console.WriteLine("DEBUG: Migrating database...");
                await _context.Database.MigrateAsync();
                Console.WriteLine("DEBUG: Migration complete.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            Console.WriteLine("DEBUG: Seeding database...");
            await ApplicationDbContextSeed.SeedAsync(_context, _userManager);
            Console.WriteLine("DEBUG: Seeding complete.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
