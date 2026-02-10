using RecipeFinder.Application.Interfaces;

namespace RecipeFinder.Application.Recipes.DeleteRecipe
{
    public class DeleteRecipeHandler
    {
        private readonly IRecipeRepository _repository;

        public DeleteRecipeHandler(IRecipeRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteRecipeCommand command)
        {
            var recipe = await _repository.GetByIdAsync(command.Id);
            if (recipe == null)
                throw new KeyNotFoundException("Recipe not found.");

            await _repository.DeleteAsync(recipe);
        }
    }
}