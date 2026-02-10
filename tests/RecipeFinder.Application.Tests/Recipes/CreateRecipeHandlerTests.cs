using Moq;
using RecipeFinder.Application.Interfaces;
using RecipeFinder.Application.Recipes.CreateRecipe;
using RecipeFinder.Domain.Entities;
using Xunit;

public class CreateRecipeHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateRecipeAndReturnId()
    {
        // Arrange
        var repoMock = new Mock<IRecipeRepository>();

        repoMock
            .Setup(r => r.AddAsync(It.IsAny<Recipe>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateRecipeHandler(repoMock.Object);

        var command = new CreateRecipeCommand(
            "Pasta",
            new List<string> { "Pasta", "Eggs" }
        );

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.NotEqual(Guid.Empty, result);

        repoMock.Verify(
            r => r.AddAsync(It.IsAny<Recipe>()),
            Times.Once
        );
    }
}
