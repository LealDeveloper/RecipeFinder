namespace RecipeFinder.Application.Recipes.SearchRecipes
{
    public class SearchRecipeCommand
    {
        public List<string> Ingredients { get; }

        public SearchRecipeCommand(List<string> ingredients)
        {
            Ingredients = ingredients;
        }
    }
}