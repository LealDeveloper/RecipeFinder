namespace RecipeFinder.Application.Recipes.CreateRecipe;

public class CreateRecipeCommand
{
    public string Name { get; }
    public List<string> Ingredients { get; }

    public CreateRecipeCommand(string name, List<string> ingredients)
    {
        Name = name;
        Ingredients = ingredients;
    }
}
