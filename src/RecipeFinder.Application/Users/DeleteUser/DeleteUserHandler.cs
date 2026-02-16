using RecipeFinder.Application.Interfaces;
using RecipeFinder.Application.Users.DeleteUser;

namespace RecipeFinder.Application.Recipes.DeleteRecipe
{
    public class DeleteUserHandler
    {
        private readonly IUserRepository _repository;

        public DeleteUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteUserCommand command)
        {
            var recipe = await _repository.GetByIdAsync(command.Id);
            if (recipe == null)
                throw new KeyNotFoundException("Recipe not found.");

            await _repository.DeleteAsync(recipe);
        }
    }
}