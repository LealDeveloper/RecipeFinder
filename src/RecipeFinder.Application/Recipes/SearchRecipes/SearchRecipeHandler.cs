using Microsoft.Extensions.Caching.Memory;
using RecipeFinder.Application.Interfaces;
using RecipeFinder.Domain.Entities;

namespace RecipeFinder.Application.Recipes.SearchRecipes
{
    public class SearchRecipesHandler
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMemoryCache _cache;

        public SearchRecipesHandler(IRecipeRepository recipeRepository, IMemoryCache cache)
        {
            _recipeRepository = recipeRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<Recipe>> Handle(SearchRecipeCommand query)
        {
            var ingredients = query.Ingredients ?? new List<string>();

            var cacheKey = "Search_" + string.Join("_", ingredients.OrderBy(i => i));

            if (_cache.TryGetValue(cacheKey, out IEnumerable<Recipe>? cachedRecipes))
            {
                return cachedRecipes ?? Enumerable.Empty<Recipe>();
            }

            var recipes = await _recipeRepository.GetByIngredientsAsync(ingredients)
                          ?? Enumerable.Empty<Recipe>();

            _cache.Set(cacheKey, recipes, TimeSpan.FromMinutes(5));

            return recipes;
        }
    }
}
