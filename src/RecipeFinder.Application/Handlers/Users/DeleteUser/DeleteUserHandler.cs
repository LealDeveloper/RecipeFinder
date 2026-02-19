using MediatR;
using RecipeFinder.Application.Common.Interfaces;

namespace RecipeFinder.Application.Handlers.Users.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUserRepository _repository;
        private readonly IIdentityService _identityService; // optional if you want to delete Identity user

        public DeleteUserHandler(IUserRepository repository, IIdentityService identityService)
        {
            _repository = repository;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken = default)
        {
            // 1️⃣ Get Domain user
            var domainUser = await _repository.GetByIdAsync(command.Id, cancellationToken);
            if (domainUser == null)
                throw new KeyNotFoundException("User not found.");

            // 2️⃣ Delete Identity user via IdentityService (optional)
            await _identityService.DeleteUserAsync(domainUser.Id);

            // 3️⃣ Delete Domain user
            await _repository.DeleteAsync(domainUser, cancellationToken);
            return Unit.Value;
        }
    }
}
