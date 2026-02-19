using MediatR;
using RecipeFinder.Domain.Entities;

public record GetAllRecipesQuery(int Page, int PageSize) : IRequest<(IEnumerable<Recipe>, int)>;

