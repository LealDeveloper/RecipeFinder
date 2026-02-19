using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeFinder.Domain.Entities;
using RecipeFinder.Infrastructure.Identity;

namespace RecipeFinder.Infrastructure.Persistence;

public class RecipeDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public RecipeDbContext(DbContextOptions<RecipeDbContext> options)
        : base(options)
    {
    }

    // Domain DbSets
    public DbSet<User> DomainUsers => Set<User>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // MUITO importante para Identity

        // Configura Domain User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.DisplayName).IsRequired();
            entity.Property(u => u.Email).IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.DisplayName).IsUnique();
        });

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

        // Configura relação Recipe ↔ Ingredient (muitos para muitos)
        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.Ingredients)
            .WithMany()
            .UsingEntity(j => j.ToTable("RecipeIngredients"));
    }
}
