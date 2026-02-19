using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecipeFinder.API.DTOs.Request.Recipe;
using RecipeFinder.API.DTOs.Response.Generic;
using RecipeFinder.API.DTOs.Response.Recipe;
using RecipeFinder.Application.Commands.Recipes.CreateRecipe;
using RecipeFinder.Application.Commands.Recipes.DeleteRecipe;
using RecipeFinder.Domain.Entities;

namespace RecipeFinder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly ISender _sender;

    public RecipesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRecipeRequest request, 
                                            CancellationToken cancellationToken)
    {
        var recipeId = await _sender.Send(new CreateRecipeCommand(request.Name, request.Ingredients), cancellationToken);

        return CreatedAtAction(nameof(GetAll), new { id = recipeId },
            new CreateRecipeResponse { Id = recipeId });
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, 
                                            [FromBody] UpdateRecipeRequest request, 
                                            CancellationToken cancellationToken = default)
    {
        await _sender.Send(new UpdateRecipeCommand(id, request.Name, request.Ingredients), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, 
                                            CancellationToken cancellationToken = default)
    {
        await _sender.Send(new DeleteRecipeCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1,
                                            [FromQuery] int pageSize = 10, 
                                            CancellationToken cancellationToken = default)
    {
        var (recipes, totalCount) = await _sender.Send(new GetAllRecipesQuery(page, pageSize), cancellationToken);
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
                                        [FromQuery] int pageSize = 10, 
                                        CancellationToken cancellationToken = default)
    {
        var (recipes, totalCount) = await _sender.Send(new SearchRecipesCommand(request.Ingredients, page, pageSize), cancellationToken);
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

