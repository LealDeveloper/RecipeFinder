using MediatR;
namespace RecipeFinder.Application.Commands.Recipes.DeleteRecipe;
public record DeleteRecipeCommand(
    Guid Id
) : IRequest<Unit>;