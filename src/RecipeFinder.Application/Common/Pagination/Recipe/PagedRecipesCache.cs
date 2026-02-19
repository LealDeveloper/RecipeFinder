using RecipeFinder.Application.Common.Pagination.Recipe;
using RecipeFinder.Domain.Entities;

public class PagedRecipesCache
{
    public List<RecipeCache> Recipes { get; set; } = new();
    public int TotalCount { get; set; }
}
