using MediatR;
using RecipeFinder.Application.Commands.Recipes.CreateRecipe;
using RecipeFinder.Application.Common.Interfaces;
using RecipeFinder.Domain.Entities;

namespace RecipeFinder.Application.Handlers.Recipes.CreateRecipe;

public class CreateRecipeHandler: IRequestHandler<CreateRecipeCommand, Guid>
{
    private readonly IRecipeRepository _recipeRepository;

    public CreateRecipeHandler(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<Guid> Handle(CreateRecipeCommand command, CancellationToken cancellationToken = default)
    {
        var recipe = new Recipe(Guid.NewGuid(), command.Name);
        foreach (var ingredientName in command.Ingredients)
        {
            recipe.AddIngredient(new Ingredient(Guid.NewGuid(), ingredientName));
        }

        await _recipeRepository.AddAsync(recipe, cancellationToken);
        return recipe.Id;
    }
}
