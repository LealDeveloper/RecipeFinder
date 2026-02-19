namespace RecipeFinder.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Guid> CreateUserAsync(string email, string displayName, string password);
        Task DeleteUserAsync(Guid userId);
    }
}
