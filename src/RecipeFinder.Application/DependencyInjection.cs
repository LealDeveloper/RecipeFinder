using Microsoft.Extensions.DependencyInjection;
using RecipeFinder.Application.Handlers.Recipes.CreateRecipe;
using RecipeFinder.Application.Handlers.Recipes.DeleteRecipe;
using RecipeFinder.Application.Handlers.Recipes.UpdateRecipe;
using RecipeFinder.Application.Handlers.Users.CreateUser;
using RecipeFinder.Application.Handlers.Users.DeleteUser;
using RecipeFinder.Application.Handlers.Users.UpdateUser;
using RecipeFinder.Application.Handlers.Users.GetUser;
using RecipeFinder.Application.Common.Interfaces;
using RecipeFinder.Application.Handlers.Recipes.GetAllRecipe;
using RecipeFinder.Application.Handlers.Recipes.SearchRecipe;
using RecipeFinder.Application.Handlers.Users.GetAllUser;
using RecipeFinder.Application.Handlers.Users.SearchUsers;
using MediatR;

namespace RecipeFinder.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(DependencyInjection).Assembly));

            return services;
        }
    }
}
