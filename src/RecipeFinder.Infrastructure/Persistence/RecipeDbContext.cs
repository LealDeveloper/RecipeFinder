using Microsoft.EntityFrameworkCore;
using RecipeFinder.Domain.Entities;

namespace RecipeFinder.Infrastructure.Persistence;

public class RecipeDbContext : DbContext
{
    public RecipeDbContext(DbContextOptions<RecipeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<User> Users => Set<User>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configura Recipe
        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();
        });

        // Configura Ingredient
        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Name).IsRequired();
        });

        // Configurar relação Recipe ↔ Ingredient (muitos para muitos)
        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.Ingredients)
            .WithMany()
            .UsingEntity(j => j.ToTable("RecipeIngredients"));
    }
}
