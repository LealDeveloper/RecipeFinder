using RecipeFinder.Domain.Entities;

namespace RecipeFinder.Application.Common.Interfaces;

public interface IRecipeRepository
{
    Task<Recipe?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Recipe recipe, CancellationToken cancellationToken = default);
    Task DeleteAsync(Recipe recipe, CancellationToken cancellationToken = default);
    Task AddAsync(Recipe recipe, CancellationToken cancellationToken = default);
    Task<IEnumerable<Recipe>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Recipe>> GetByIngredientsAsync(List<string> ingredients, CancellationToken cancellationToken = default);
    Task<(List<Recipe> Recipes, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<(List<Recipe> Recipes, int TotalCount)> GetByIngredientsPagedAsync(List<string> ingredients, int page, int pageSize, CancellationToken cancellationToken = default);
}
