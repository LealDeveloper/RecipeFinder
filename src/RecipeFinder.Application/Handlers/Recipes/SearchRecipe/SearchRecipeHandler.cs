using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using RecipeFinder.Application.Common.Interfaces;
using RecipeFinder.Application.Common.Pagination.Recipe;
using RecipeFinder.Domain.Entities;
using System.Text.Json;

namespace RecipeFinder.Application.Handlers.Recipes.SearchRecipe
{
    // DTO simples para armazenar no cache


    public class SearchRecipesHandler : IRequestHandler<SearchRecipesCommand, (IEnumerable<Recipe>, int)>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IDistributedCache _cache;

        public SearchRecipesHandler(IRecipeRepository recipeRepository, IDistributedCache cache)
        {
            _recipeRepository = recipeRepository;
            _cache = cache;
        }

        public async Task<(IEnumerable<Recipe>, int)> Handle(SearchRecipesCommand command, CancellationToken cancellationToken = default)
        {
            var cacheKey = BuildCacheKey(command.Ingredients, command.Page, command.PageSize);

            // Tenta buscar do cache
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

            // Busca do repositório com paginação
            var (recipes, totalCount) = await _recipeRepository.GetByIngredientsPagedAsync(
                command.Ingredients, command.Page, command.PageSize, cancellationToken);

            // Converte para DTOs antes de guardar no cache
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

        private static string BuildCacheKey(List<string> ingredients, int page, int pageSize)
        {
            var keyIngredients = string.Join("-", ingredients.OrderBy(i => i));
            return $"search:{keyIngredients}:page:{page}:size:{pageSize}";
        }
    }
}
