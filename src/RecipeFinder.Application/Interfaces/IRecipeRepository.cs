using RecipeFinder.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeFinder.Application.Interfaces;

public interface IRecipeRepository
{
    Task<IEnumerable<Recipe>> GetAllAsync();
    Task AddAsync(Recipe recipe);
    Task<IEnumerable<Recipe>> GetByIngredientsAsync(List<string> ingredients);
    Task<(List<Recipe> Recipes, int TotalCount)> GetPagedAsync(int page, int pageSize);
    Task<(List<Recipe> Recipes, int TotalCount)> GetByIngredientsPagedAsync(List<string> ingredients, int page, int pageSize);
    Task<Recipe?> GetByIdAsync(Guid id);
    Task UpdateAsync(Recipe recipe);
    Task DeleteAsync(Recipe recipe);
}
