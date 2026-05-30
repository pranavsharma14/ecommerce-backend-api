using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CleanAPI.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
    "Data Source=localhost;Initial Catalog=CleanAPIDb;Integrated Security=True;TrustServerCertificate=True");

        return new AppDbContext(optionsBuilder.Options);
    }
}