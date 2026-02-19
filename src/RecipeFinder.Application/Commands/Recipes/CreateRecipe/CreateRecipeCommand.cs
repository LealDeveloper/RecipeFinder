using MediatR;
namespace RecipeFinder.Application.Commands.Recipes.CreateRecipe;
public record CreateRecipeCommand(
    string Name,
    List<string> Ingredients
) : IRequest<Guid>;