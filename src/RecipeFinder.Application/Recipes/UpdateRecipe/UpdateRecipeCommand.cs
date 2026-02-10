namespace RecipeFinder.Application.Recipes.UpdateRecipe
{
    public class UpdateRecipeCommand
    {
        public Guid Id { get; }
        public string Name { get; }
        public List<string> Ingredients { get; }

        public UpdateRecipeCommand(Guid id, string name, List<string> ingredients)
        {
            Id = id;
            Name = name;
            Ingredients = ingredients;
        }
    }
}
