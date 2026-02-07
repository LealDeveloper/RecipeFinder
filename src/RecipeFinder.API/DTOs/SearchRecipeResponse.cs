namespace RecipeFinder.API.DTOs
{
    public class SearchRecipeResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new();
    }
}
