namespace RecipeFinder.Domain.Entities;

public class Ingredient
{
    public Guid Id { get; private set; }
    public string? Name { get; private set; }

    private Ingredient() { }

    public Ingredient(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
