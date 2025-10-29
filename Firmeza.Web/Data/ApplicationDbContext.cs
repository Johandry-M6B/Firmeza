using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    
}