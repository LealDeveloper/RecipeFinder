namespace RecipeFinder.API.DTOs.Response.Recipe;

public class RecipeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> Ingredients { get; set; } = new();
}
