using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeFinder.Application.Interfaces;
using RecipeFinder.Application.Recipes.CreateRecipe;
using RecipeFinder.Application.Recipes.SearchRecipes;
using RecipeFinder.Infrastructure.Persistence;
using RecipeFinder.Infrastructure.Repositories;

namespace RecipeFinder.API.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurar EF Core com PostgreSQL
            services.AddDbContext<RecipeDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Repositórios
            services.AddScoped<IRecipeRepository, RecipeRepository>();

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Handlers
            services.AddScoped<CreateRecipeHandler>();
            services.AddScoped<SearchRecipesHandler>();
            return services;
        }
    }
}
