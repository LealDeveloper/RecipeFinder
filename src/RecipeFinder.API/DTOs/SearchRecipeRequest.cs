namespace RecipeFinder.API.DTOs
{
    public class SearchRecipeRequest
    {
        public List<string> Ingredients { get; set; } = new();
    }
}
