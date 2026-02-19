using Microsoft.EntityFrameworkCore;
using RecipeFinder.Application.Common.Interfaces;
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
    public async Task<Recipe?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Recipe recipe, CancellationToken cancellationToken = default)
    {
        _context.Recipes.Update(recipe);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Recipe recipe, CancellationToken cancellationToken = default)
    {
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<IEnumerable<Recipe>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Recipe recipe, CancellationToken cancellationToken = default)
    {
        await _context.Recipes.AddAsync(recipe, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Recipe>> GetByIngredientsAsync(List<string> ingredients, CancellationToken cancellationToken = default)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .Where(r => ingredients.All(i => r.Ingredients.Select(ing => ing.Name).Contains(i)))
            .ToListAsync(cancellationToken);
    }
    public async Task<(List<Recipe> Recipes, int TotalCount)> GetPagedAsync(int page,int pageSize, CancellationToken cancellationToken = default)
    {
        var baseQuery = _context.Recipes.AsNoTracking();

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var recipeIds = await baseQuery
            .OrderBy(r => r.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => r.Id)
            .ToListAsync(cancellationToken);

        var recipes = await _context.Recipes
            .AsNoTracking()
            .Where(r => recipeIds.Contains(r.Id))
            .Include(r => r.Ingredients)
            .ToListAsync(cancellationToken);

        return (recipes, totalCount);
    }


    public async Task<(List<Recipe> Recipes, int TotalCount)> GetByIngredientsPagedAsync(List<string> ingredients,int page,int pageSize, CancellationToken cancellationToken = default)
    {
        var baseQuery = _context.Recipes
            .AsNoTracking()
            .Where(r =>
                ingredients.All(i =>
                    r.Ingredients.Any(ing => ing.Name == i)));

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var recipeIds = await baseQuery
            .OrderBy(r => r.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => r.Id)
            .ToListAsync(cancellationToken);

        var recipes = await _context.Recipes
            .AsNoTracking()
            .Where(r => recipeIds.Contains(r.Id))
            .Include(r => r.Ingredients)
            .ToListAsync(cancellationToken);

        return (recipes, totalCount);
    }

}
