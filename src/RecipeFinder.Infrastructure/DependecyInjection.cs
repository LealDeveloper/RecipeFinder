using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeFinder.Application.Interfaces;
using RecipeFinder.Infrastructure.Persistence;
using RecipeFinder.Infrastructure.Repositories;

namespace RecipeFinder.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<RecipeDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();


            return services;
        }

    }
}
