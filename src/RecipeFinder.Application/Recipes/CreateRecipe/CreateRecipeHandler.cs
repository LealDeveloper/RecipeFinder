using RecipeFinder.Application.Interfaces;
using RecipeFinder.Domain.Entities;

namespace RecipeFinder.Application.Recipes.CreateRecipe;

public class CreateRecipeHandler
{
    private readonly IRecipeRepository _recipeRepository;

    public CreateRecipeHandler(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<Guid> Handle(CreateRecipeCommand command)
    {
        var recipe = new Recipe(Guid.NewGuid(), command.Name);
        foreach (var ingredientName in command.Ingredients)
        {
            recipe.AddIngredient(new Ingredient(Guid.NewGuid(), ingredientName));
        }

        await _recipeRepository.AddAsync(recipe);
        return recipe.Id;
    }
}
