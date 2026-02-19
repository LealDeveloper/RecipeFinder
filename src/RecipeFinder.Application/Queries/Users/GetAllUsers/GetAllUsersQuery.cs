using MediatR;
using RecipeFinder.Domain.Entities;

public record GetAllUsersQuery(int Page = 1, int PageSize = 10) : IRequest<(IEnumerable<User>, int)>;