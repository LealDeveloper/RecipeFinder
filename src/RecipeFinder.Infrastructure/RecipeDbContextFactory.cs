using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RecipeFinder.Infrastructure.Persistence;

public class RecipeDbContextFactory : IDesignTimeDbContextFactory<RecipeDbContext>
{
    public RecipeDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<RecipeDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

        return new RecipeDbContext(optionsBuilder.Options);
    }
}
