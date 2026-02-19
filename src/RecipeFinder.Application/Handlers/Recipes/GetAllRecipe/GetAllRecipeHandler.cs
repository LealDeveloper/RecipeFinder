using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using RecipeFinder.Application.Common.Interfaces;
using RecipeFinder.Application.Common.Pagination.Recipe;
using RecipeFinder.Domain.Entities;
using System.Linq;
using System.Text.Json;

namespace RecipeFinder.Application.Handlers.Recipes.GetAllRecipe
{


    public class GetAllRecipesHandler : IRequestHandler<GetAllRecipesQuery, (IEnumerable<Recipe>, int)>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IDistributedCache _cache;

        public GetAllRecipesHandler(IRecipeRepository recipeRepository, IDistributedCache cache)
        {
            _recipeRepository = recipeRepository;
            _cache = cache;
        }

        public async Task<(IEnumerable<Recipe>, int)> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"all_recipes:page:{request.Page}:size:{request.PageSize}";

            var cached = await _cache.GetStringAsync(cacheKey);
            if (cached != null)
            {
                var cachedObj = JsonSerializer.Deserialize<PagedRecipesCache>(cached)!;
                // Converte de volta para entidades do domínio
                var recipesFromCache = cachedObj.Recipes.Select(r => new Recipe(
                    r.Id,
                    r.Name,
                    r.Ingredients.Select(i => new Ingredient(Guid.NewGuid(), i)).ToList()
                ));
                return (recipesFromCache, cachedObj.TotalCount);
            }

            // Busca paginada no repositório
            (var recipes, var totalCount) = await _recipeRepository.GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            var cacheObj = new PagedRecipesCache
            {
                Recipes = recipes.Select(r => new RecipeCache
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
