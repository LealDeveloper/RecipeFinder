namespace RecipeFinder.Application.Common.Pagination.User
{
    public class UserCache
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
