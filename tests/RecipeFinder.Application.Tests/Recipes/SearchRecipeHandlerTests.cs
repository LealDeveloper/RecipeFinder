using Microsoft.Extensions.Caching.Memory;
using Moq;
using RecipeFinder.Application.Interfaces;
using RecipeFinder.Application.Recipes.SearchRecipes;
using RecipeFinder.Domain.Entities;
using Xunit;

public class SearchRecipesHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnRecipesFromRepository()
    {
        // Arrange
        var repoMock = new Mock<IRecipeRepository>();

        var recipes = new List<Recipe>
        {
            new Recipe(Guid.NewGuid(), "Omelette")
        };

        repoMock
            .Setup(r => r.GetByIngredientsAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(recipes);

        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var handler = new SearchRecipesHandler(repoMock.Object, memoryCache);

        var query = new SearchRecipeCommand(new List<string> { "Eggs" });

        // Act
        var result = await handler.Handle(query);

        // Assert
        Assert.Single(result);
        Assert.Equal("Omelette", result.First().Name);
    }
}
