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
    public async Task<Recipe?> GetByIdAsync(Guid id)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task UpdateAsync(Recipe recipe)
    {
        _context.Recipes.Update(recipe);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Recipe recipe)
    {
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
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
    public async Task<(List<Recipe> Recipes, int TotalCount)> GetPagedAsync(int page,int pageSize)
    {
        var baseQuery = _context.Recipes.AsNoTracking();

        var totalCount = await baseQuery.CountAsync();

        var recipeIds = await baseQuery
            .OrderBy(r => r.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => r.Id)
            .ToListAsync();

        var recipes = await _context.Recipes
            .AsNoTracking()
            .Where(r => recipeIds.Contains(r.Id))
            .Include(r => r.Ingredients)
            .ToListAsync();

        return (recipes, totalCount);
    }


    public async Task<(List<Recipe> Recipes, int TotalCount)> GetByIngredientsPagedAsync(List<string> ingredients,int page,int pageSize)
    {
        var baseQuery = _context.Recipes
            .AsNoTracking()
            .Where(r =>
                ingredients.All(i =>
                    r.Ingredients.Any(ing => ing.Name == i)));

        var totalCount = await baseQuery.CountAsync();

        var recipeIds = await baseQuery
            .OrderBy(r => r.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => r.Id)
            .ToListAsync();

        var recipes = await _context.Recipes
            .AsNoTracking()
            .Where(r => recipeIds.Contains(r.Id))
            .Include(r => r.Ingredients)
            .ToListAsync();

        return (recipes, totalCount);
    }

}
