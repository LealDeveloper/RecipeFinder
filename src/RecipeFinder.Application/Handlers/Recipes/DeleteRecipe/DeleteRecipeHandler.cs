using MediatR;
using RecipeFinder.Application.Commands.Recipes.DeleteRecipe;
using RecipeFinder.Application.Common.Interfaces;

namespace RecipeFinder.Application.Handlers.Recipes.DeleteRecipe
{
    public class DeleteRecipeHandler : IRequestHandler<DeleteRecipeCommand, Unit>
    {
        private readonly IRecipeRepository _repository;

        public DeleteRecipeHandler(IRecipeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteRecipeCommand command, CancellationToken cancellationToken = default)
        {
            var recipe = await _repository.GetByIdAsync(command.Id, cancellationToken);
            if (recipe == null)
                throw new KeyNotFoundException("Recipe not found.");

            await _repository.DeleteAsync(recipe, cancellationToken);
            return Unit.Value;
        }
    }
}