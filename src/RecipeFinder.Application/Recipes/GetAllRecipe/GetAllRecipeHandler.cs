using Microsoft.Extensions.Caching.Distributed;
using RecipeFinder.Application.Interfaces;
using RecipeFinder.Domain.Entities;
using System.Linq;
using System.Text.Json;

namespace RecipeFinder.Application.Recipes.GetAllRecipes
{
    public class RecipeCacheDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public List<string> Ingredients { get; set; } = new();
    }

    public class PagedRecipesCacheDto
    {
        public List<RecipeCacheDto> Recipes { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class GetAllRecipesHandler
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IDistributedCache _cache;

        public GetAllRecipesHandler(IRecipeRepository recipeRepository, IDistributedCache cache)
        {
            _recipeRepository = recipeRepository;
            _cache = cache;
        }

        public async Task<(IEnumerable<Recipe>, int)> Handle(int page, int pageSize)
        {
            var cacheKey = $"all_recipes:page:{page}:size:{pageSize}";

            var cached = await _cache.GetStringAsync(cacheKey);
            if (cached != null)
            {
                var cachedObj = JsonSerializer.Deserialize<PagedRecipesCacheDto>(cached)!;
                // Converte de volta para entidades do domínio
                var recipesFromCache = cachedObj.Recipes.Select(r => new Recipe(
                    r.Id,
                    r.Name,
                    r.Ingredients.Select(i => new Ingredient(Guid.NewGuid(), i)).ToList()
                ));
                return (recipesFromCache, cachedObj.TotalCount);
            }

            // Busca paginada no repositório
            (var recipes, var totalCount) = await _recipeRepository.GetPagedAsync(page, pageSize);

            var cacheObj = new PagedRecipesCacheDto
            {
                Recipes = recipes.Select(r => new RecipeCacheDto
                {
                    Id = r.Id,
                    Name = r.Name!,
                    Ingredients = r.Ingredients.Select(i => i.Name!).ToList()
                }).ToList(),
                TotalCount = totalCount
            };

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(cacheObj),
                options);

            return (recipes, totalCount);
        }
    }
}
