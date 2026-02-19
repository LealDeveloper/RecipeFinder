using RecipeFinder.Domain.Entities.Common;

namespace RecipeFinder.Domain.Entities;

public class Ingredient : Entity<Guid>
{
    public string Name { get; private set; } = null!;

    protected Ingredient() { } // EF Core

    // Construtor principal
    public Ingredient(Guid id, string name) : base(id)
    {
        Name = name;
    }

    // Construtor simplificado
    public Ingredient(string name) : this(Guid.NewGuid(), name) { }

    // Método para alterar o nome se necessário
    public void UpdateName(string name)
    {
        Name = name;
    }
}
