using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Firmeza.Web.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Configuración para migraciones en tiempo de diseño
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Connection string usando Connection Pooler (región us-east-2)
            var connectionString = "Host=aws-1-us-east-2.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.wubmfoxhgtvcbgaimmcg;Password=1001877889Ka;SSL Mode=Require;Trust Server Certificate=true;Pooling=true;Minimum Pool Size=1;Maximum Pool Size=5;Timeout=30";
            
            optionsBuilder.UseNpgsql(connectionString);
            
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}