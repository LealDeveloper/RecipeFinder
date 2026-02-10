namespace RecipeFinder.Domain.Entities;

public class Recipe
{
    public Guid Id { get; private set; }
    public string? Name { get; private set; }

    // Lista de ingredientes (inicialmente vazia)
    public List<Ingredient> Ingredients { get; private set; } = new();

    private Recipe() { } // necessário para EF ou deserialização

    public Recipe(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    public Recipe(string name, List<Ingredient> ingredients)
    {
        Id = Guid.NewGuid();
        Name = name;
        Ingredients = ingredients;
    }
    public Recipe(Guid id, string name, List<Ingredient> ingredients)
    {
        Id = id;
        Name = name;
        Ingredients = ingredients;
    }

    public void Update(string name, List<Ingredient> ingredients)
    {
        Name = name;
        Ingredients = ingredients;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        Ingredients.Add(ingredient);
    }
}
