namespace RecipeFinder.Application.Recipes.SearchRecipes
{
    public class SearchRecipesCommand
    {
        public List<string> Ingredients { get; }
        public int Page { get; }
        public int PageSize { get; }

        public SearchRecipesCommand(List<string> ingredients, int page = 1, int pageSize = 10)
        {
            Ingredients = ingredients ?? new List<string>();
            Page = page;
            PageSize = pageSize;
        }
    }
}