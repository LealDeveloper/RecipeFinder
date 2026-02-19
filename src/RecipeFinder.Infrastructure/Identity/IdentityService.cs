using Microsoft.AspNetCore.Identity;
using RecipeFinder.Application.Common.Interfaces;
using RecipeFinder.Infrastructure.Identity;

namespace RecipeFinder.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Guid> CreateUserAsync(string email, string displayName, string password)
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                throw new InvalidOperationException("Email already in use.");

            var identityUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(identityUser, password);

            if (!result.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            return identityUser.Id;
        }
        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return; // or throw

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
        }
    }
}
