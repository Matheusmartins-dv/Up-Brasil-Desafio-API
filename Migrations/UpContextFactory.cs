using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Migrations;

public class UpContextFactory : IDesignTimeDbContextFactory<UpContext>
{
    public UpContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UpContext>();

        var connectionString = "Host=localhost;Port=5432;Database=updb;Username=updb;Password=postgres123";

        optionsBuilder.UseNpgsql(connectionString,
            b => b.MigrationsAssembly("Migrations"));

        return new UpContext(optionsBuilder.Options);
    }

}
