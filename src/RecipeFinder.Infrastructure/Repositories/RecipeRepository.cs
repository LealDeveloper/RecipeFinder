using Microsoft.EntityFrameworkCore;
using RecipeFinder.Application.Interfaces;
using RecipeFinder.Domain.Entities;
using RecipeFinder.Infrastructure.Persistence;

namespace RecipeFinder.Infrastructure.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly RecipeDbContext _context;

    public RecipeRepository(RecipeDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Recipe>> GetAllAsync()
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .ToListAsync();
    }

    public async Task AddAsync(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Recipe>> GetByIngredientsAsync(List<string> ingredients)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .Where(r => ingredients.All(i => r.Ingredients.Select(ing => ing.Name).Contains(i)))
            .ToListAsync();
    }
    public async Task<(IEnumerable<Recipe> Recipes, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var query = _context.Recipes.Include(r => r.Ingredients).AsQueryable();
        var totalCount = await query.CountAsync();

        var recipes = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (recipes, totalCount);
    }

    public async Task<(IEnumerable<Recipe> Recipes, int TotalCount)> GetByIngredientsPagedAsync(List<string> ingredients, int page, int pageSize)
    {
        var query = _context.Recipes
            .Include(r => r.Ingredients)
            .Where(r => ingredients.All(i => r.Ingredients.Select(ing => ing.Name).Contains(i)));

        var totalCount = await query.CountAsync();

        var recipes = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (recipes, totalCount);
    }
}
