namespace RecipeFinder.Application.Common.Pagination.Recipe
{
    public class RecipeCache
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public List<string> Ingredients { get; set; } = new();
    }
}
