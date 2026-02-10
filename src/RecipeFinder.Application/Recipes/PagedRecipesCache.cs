using RecipeFinder.Domain.Entities;

public class PagedRecipesCache
{
    public List<Recipe> Recipes { get; set; } = new();
    public int TotalCount { get; set; }
}