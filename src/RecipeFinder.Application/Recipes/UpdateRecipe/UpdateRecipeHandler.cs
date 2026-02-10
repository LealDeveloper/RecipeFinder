using RecipeFinder.Application.Interfaces;
using RecipeFinder.Domain.Entities;

namespace RecipeFinder.Application.Recipes.UpdateRecipe
{
    public class UpdateRecipeHandler
    {
        private readonly IRecipeRepository _repository;

        public UpdateRecipeHandler(IRecipeRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateRecipeCommand command)
        {
            var recipe = await _repository.GetByIdAsync(command.Id);
            if (recipe == null)
                throw new KeyNotFoundException("Recipe not found.");

            var ingredients = command.Ingredients
                            .Select(i => new Ingredient(Guid.NewGuid(), i))
                            .ToList();
            recipe.Update(command.Name, ingredients);

            await _repository.UpdateAsync(recipe);
        }
    }
}
