namespace RecipeFinder.API.DTOs.Request.Recipe
{
    public class UpdateRecipeRequest
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new();
    }

}
