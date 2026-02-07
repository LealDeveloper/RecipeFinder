using Microsoft.AspNetCore.Mvc;
using RecipeFinder.API.DTOs;
using RecipeFinder.Application.Interfaces;
using RecipeFinder.Application.Recipes.CreateRecipe;
using RecipeFinder.Application.Recipes.SearchRecipes;

namespace RecipeFinder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly CreateRecipeHandler _createRecipeHandler;
    private readonly IRecipeRepository _recipeRepository;
    private readonly SearchRecipesHandler _searchRecipesHandler;

    public RecipesController(CreateRecipeHandler createRecipeHandler,
                             IRecipeRepository recipeRepository,
                             SearchRecipesHandler searchRecipesHandler)
    {
        _createRecipeHandler = createRecipeHandler;
        _recipeRepository = recipeRepository;
        _searchRecipesHandler = searchRecipesHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRecipeRequest request)
    {
        var command = new CreateRecipeCommand(request.Name, request.Ingredients);
        var recipeId = await _createRecipeHandler.Handle(command);

        var response = new CreateRecipeResponse { Id = recipeId };
        return CreatedAtAction(nameof(GetAll), new { id = recipeId }, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var allRecipes = await _recipeRepository.GetAllAsync();

        var pagedRecipes = allRecipes
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RecipeResponse
            {
                Id = r.Id,
                Name = r.Name!,
                Ingredients = r.Ingredients.Select(i => i.Name!).ToList()
            })
            .ToList();

        var response = new PagedResponse<RecipeResponse>
        {
            Items = pagedRecipes,
            Page = page,
            PageSize = pageSize,
            TotalCount = allRecipes.Count()
        };

        return Ok(response);
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchRecipeRequest request,
                                            [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var recipes = await _searchRecipesHandler.Handle(new SearchRecipeCommand(request.Ingredients));

        var pagedRecipes = recipes
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RecipeResponse
            {
                Id = r.Id,
                Name = r.Name!,
                Ingredients = r.Ingredients.Select(i => i.Name!).ToList()
            })
            .ToList();

        var response = new PagedResponse<RecipeResponse>
        {
            Items = pagedRecipes,
            Page = page,
            PageSize = pageSize,
            TotalCount = recipes.Count()
        };

        return Ok(response);
    }
}
