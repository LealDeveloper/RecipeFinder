using MediatR;

namespace RecipeFinder.Application.Commands.Users.CreateUser;

public record CreateUserCommand(string DisplayName, string Email, string Password) : IRequest<Guid>;