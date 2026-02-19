using MediatR;

public record UpdateRecipeCommand(
    Guid Id,
    string Name,
    List<string> Ingredients
) : IRequest<Unit>;