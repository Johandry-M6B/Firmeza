using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public class InitialiserHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InitialiserHostedService> _logger;

    public InitialiserHostedService(
        IServiceProvider serviceProvider,
        ILogger<InitialiserHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
        
        Console.WriteLine($"DEBUG: HostedService started. Environment: {env.EnvironmentName}");

        // if (env.IsDevelopment()) // Commented out for debugging
        {
            try
            {
                Console.WriteLine("DEBUG: Calling Initialiser...");
                var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
                await initialiser.InitialiseAsync();
                await initialiser.SeedAsync();
                Console.WriteLine("DEBUG: Initialiser finished.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: Initialiser failed: {ex}");
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
