using MediatR;
using RecipeFinder.Application.Common.Interfaces;
using RecipeFinder.Domain.Entities;

namespace RecipeFinder.Application.Handlers.Recipes.UpdateRecipe
{
    public class UpdateRecipeHandler : IRequestHandler<UpdateRecipeCommand,Unit>
    {
        private readonly IRecipeRepository _repository;

        public UpdateRecipeHandler(IRecipeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateRecipeCommand command, CancellationToken cancellationToken = default)
        {
            var recipe = await _repository.GetByIdAsync(command.Id, cancellationToken);
            if (recipe == null)
                throw new KeyNotFoundException("Recipe not found.");

            var ingredients = command.Ingredients
                            .Select(i => new Ingredient(Guid.NewGuid(), i))
                            .ToList();
            recipe.Update(command.Name, ingredients);

            await _repository.UpdateAsync(recipe, cancellationToken);
            return Unit.Value;
        }
    }
}
