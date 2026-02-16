namespace RecipeFinder.API.DTOs.Request.Recipe
{
    public class SearchRecipeRequest
    {
        public List<string> Ingredients { get; set; } = new();
    }
}
