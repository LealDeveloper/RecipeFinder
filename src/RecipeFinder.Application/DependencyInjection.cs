using Microsoft.Extensions.DependencyInjection;
using RecipeFinder.Application.Recipes.CreateRecipe;

namespace RecipeFinder.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateRecipeHandler>();
            return services;
        }
    }
}
