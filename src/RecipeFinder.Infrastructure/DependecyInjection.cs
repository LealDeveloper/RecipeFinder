using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeFinder.Application.Common.Interfaces;
using RecipeFinder.Infrastructure.Identity;
using RecipeFinder.Infrastructure.Persistence;
using RecipeFinder.Infrastructure.Repositories;
using RecipeFinder.Infrastructure.Services;

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
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                        .AddEntityFrameworkStores<RecipeDbContext>()
                        .AddDefaultTokenProviders();
            // IdentityService
            services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }

    }
}
