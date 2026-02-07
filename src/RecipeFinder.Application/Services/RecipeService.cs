using RecipeFinder.Domain.Entities;

namespace RecipeFinder.Application.Services;

public class RecipeService
{
    public IEnumerable<Recipe> GetRecipes()
    {
        var egg = new Ingredient(Guid.NewGuid(), "Egg");
        var flour = new Ingredient(Guid.NewGuid(), "Flour");

        var recipe1 = new Recipe(Guid.NewGuid(), "Omelette");
        recipe1.AddIngredient(egg);

        var recipe2 = new Recipe(Guid.NewGuid(), "Pancake");
        recipe2.AddIngredient(egg);
        recipe2.AddIngredient(flour);

        return new[] { recipe1, recipe2 };
    }
}
