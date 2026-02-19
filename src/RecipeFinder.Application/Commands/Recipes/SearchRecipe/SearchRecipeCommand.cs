using MediatR;
using RecipeFinder.Domain.Entities;

public record SearchRecipesCommand(
    List<string> Ingredients,
    int Page,
    int PageSize
) : IRequest<(IEnumerable<Recipe>, int)>;
