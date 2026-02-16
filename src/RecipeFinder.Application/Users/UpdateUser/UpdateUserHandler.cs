using RecipeFinder.Application.Interfaces;
using RecipeFinder.Domain.Entities;

namespace RecipeFinder.Application.Users.UpdateUser
{
    public class UpdateUserHandler
    {
        private readonly IUserRepository _repository;

        public UpdateUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateUserCommand command)
        {
            var user = await _repository.GetByIdAsync(command.Id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            user.UpdateProfile(command.DisplayName, command.Email);

            await _repository.UpdateAsync(user);
        }
    }
}
