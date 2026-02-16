using Microsoft.Extensions.DependencyInjection;
using RecipeFinder.Application.Interfaces;
using RecipeFinder.Application.Recipes.CreateRecipe;
using RecipeFinder.Application.Recipes.DeleteRecipe;
using RecipeFinder.Application.Recipes.GetAllRecipes;
using RecipeFinder.Application.Recipes.SearchRecipes;
using RecipeFinder.Application.Recipes.UpdateRecipe;
using RecipeFinder.Application.Users.CreateUser;
using RecipeFinder.Application.Users.GetAllUser;
using RecipeFinder.Application.Users.SearchUser;
using RecipeFinder.Application.Users.UpdateUser;

namespace RecipeFinder.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateRecipeHandler>();
            services.AddScoped<GetAllRecipesHandler>();
            services.AddScoped<SearchRecipesHandler>();
            services.AddScoped<UpdateRecipeHandler>();
            services.AddScoped<DeleteRecipeHandler>();
            services.AddScoped<CreateUserHandler>();
            services.AddScoped<DeleteUserHandler>();
            services.AddScoped<GetAllUserHandler>();
            services.AddScoped<SearchUserHandler>();
            services.AddScoped<UpdateUserHandler>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            return services;
        }
    }
}
