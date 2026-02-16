namespace RecipeFinder.API.DTOs.Request.Recipe
{
    public class CreateRecipeRequest
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new();
    }

}
