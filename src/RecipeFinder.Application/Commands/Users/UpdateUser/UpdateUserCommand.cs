using MediatR;

public record UpdateUserCommand(Guid Id, string DisplayName, string Email) : IRequest<Unit>;