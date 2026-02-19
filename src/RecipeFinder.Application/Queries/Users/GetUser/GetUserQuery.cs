using MediatR;
using RecipeFinder.Domain.Entities;
public record GetUserQuery(Guid Id) : IRequest<User>;