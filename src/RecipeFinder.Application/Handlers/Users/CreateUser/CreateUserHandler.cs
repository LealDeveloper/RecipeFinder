using MediatR;
using RecipeFinder.Application.Commands.Users.CreateUser;
using RecipeFinder.Application.Common.Interfaces;
using RecipeFinder.Domain.Entities;

namespace RecipeFinder.Application.Handlers.Users.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityService;

        public CreateUserHandler(IUserRepository userRepository, IIdentityService identityService)
        {
            _userRepository = userRepository;
            _identityService = identityService;
        }

        public async Task<Guid> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            // 1️⃣ Check if the display name already exists in Domain
            var nicknameExists = await _userRepository.GetByDisplayNameAsync(command.DisplayName, cancellationToken);
            if (nicknameExists != null)
                throw new InvalidOperationException("Nickname already in use.");

            // 2️⃣ Delegate creation of Identity user to Infrastructure
            var userId = await _identityService.CreateUserAsync(
                command.Email,
                command.DisplayName,
                command.Password
            );

            // 3️⃣ Create Domain user linked to Identity user
            var domainUser = new User(userId, command.Email, command.DisplayName);
            await _userRepository.AddAsync(domainUser, cancellationToken);

            return domainUser.Id;
        }
    }
}