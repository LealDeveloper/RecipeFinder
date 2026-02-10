using Microsoft.AspNetCore.Mvc;
using RecipeFinder.API.DTOs;
using RecipeFinder.Application.Interfaces;
using RecipeFinder.Application.Recipes.CreateRecipe;
using RecipeFinder.Application.Recipes.DeleteRecipe;
using RecipeFinder.Application.Recipes.GetAllRecipes;
using RecipeFinder.Application.Recipes.SearchRecipes;
using RecipeFinder.Application.Recipes.UpdateRecipe;
using RecipeFinder.Domain.Entities;
using RecipeFinder.Infrastructure.Repositories;

namespace RecipeFinder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly CreateRecipeHandler _createRecipeHandler;
    private readonly GetAllRecipesHandler _getAllRecipesHandler;
    private readonly SearchRecipesHandler _searchRecipesHandler;
    private readonly UpdateRecipeHandler _updateRecipeHandler;
    private readonly DeleteRecipeHandler _deleteRecipeHandler;

    public RecipesController(CreateRecipeHandler createRecipeHandler,
                         SearchRecipesHandler searchRecipesHandler,
                         GetAllRecipesHandler getAllRecipesHandler,
                         UpdateRecipeHandler updateRecipeHandler,
                         DeleteRecipeHandler deleteRecipeHandler)
    {
        _createRecipeHandler = createRecipeHandler;
        _searchRecipesHandler = searchRecipesHandler;
        _getAllRecipesHandler = getAllRecipesHandler;
        _updateRecipeHandler = updateRecipeHandler;
        _deleteRecipeHandler = deleteRecipeHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRecipeRequest request)
    {
        var recipeId = await _createRecipeHandler.Handle(
            new CreateRecipeCommand(request.Name, request.Ingredients));

        return CreatedAtAction(nameof(GetAll), new { id = recipeId },
            new CreateRecipeResponse { Id = recipeId });
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRecipeRequest request)
    {
        await _updateRecipeHandler.Handle(new UpdateRecipeCommand(id, request.Name, request.Ingredients));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteRecipeHandler.Handle(new DeleteRecipeCommand(id));
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var (recipes, totalCount) = await _getAllRecipesHandler.Handle(page, pageSize);
        var items = recipes.Select(Map).ToList();
        var response = new PagedResponse<RecipeResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return Ok(response);
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchRecipeRequest request,
                                        [FromQuery] int page = 1,
                                        [FromQuery] int pageSize = 10)
    {
        var command = new SearchRecipesCommand(request.Ingredients, page, pageSize);
        var (recipes, totalCount) = await _searchRecipesHandler.Handle(command);
        var items = recipes.Select(Map).ToList();
        var response = new PagedResponse<RecipeResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return Ok(response);
    }


    private static RecipeResponse Map(Recipe r) => new()
    {
        Id = r.Id,
        Name = r.Name!,
        Ingredients = r.Ingredients.Select(i => i.Name!).ToList()
    };
}

