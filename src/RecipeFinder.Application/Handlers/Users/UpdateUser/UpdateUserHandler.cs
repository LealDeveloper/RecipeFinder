using MediatR;
using RecipeFinder.Application.Common.Interfaces;
using RecipeFinder.Domain.Entities;

namespace RecipeFinder.Application.Handlers.Users.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserRepository _repository;

        public UpdateUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _repository.GetByIdAsync(command.Id, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            user.UpdateProfile(command.DisplayName, command.Email);

            await _repository.UpdateAsync(user, cancellationToken);
            return Unit.Value;
        }
    }
}
