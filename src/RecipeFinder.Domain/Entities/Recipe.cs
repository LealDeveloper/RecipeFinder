using RecipeFinder.Domain.Entities.Common;

namespace RecipeFinder.Domain.Entities;

public class Recipe : Entity<Guid>
{
    public string Name { get; private set; } = null!;

    private readonly List<Ingredient> _ingredients = new();
    public IReadOnlyList<Ingredient> Ingredients => _ingredients.AsReadOnly();

    protected Recipe() { } // EF Core

    // Construtor principal
    public Recipe(Guid id, string name, List<Ingredient>? ingredients = null) : base(id)
    {
        Name = name;
        if (ingredients != null)
            _ingredients.AddRange(ingredients);
    }

    // Construtor simplificado sem id
    public Recipe(string name, List<Ingredient>? ingredients = null)
        : this(Guid.NewGuid(), name, ingredients) { }

    // Métodos de modificação
    public void Update(string name, List<Ingredient> ingredients)
    {
        Name = name;
        _ingredients.Clear();
        _ingredients.AddRange(ingredients);
    }

    public void AddIngredient(Ingredient ingredient)
    {
        _ingredients.Add(ingredient);
    }
}
