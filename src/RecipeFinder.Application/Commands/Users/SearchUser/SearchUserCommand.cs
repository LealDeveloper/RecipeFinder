using MediatR;
using RecipeFinder.Domain.Entities;

public record SearchUserCommand(string DisplayName, string Email, int Page, int PageSize) : IRequest<(IEnumerable<User>, int)>;