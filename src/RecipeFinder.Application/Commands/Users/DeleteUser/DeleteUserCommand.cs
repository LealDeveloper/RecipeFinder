using MediatR;
public record DeleteUserCommand(Guid Id) : IRequest<Unit>;