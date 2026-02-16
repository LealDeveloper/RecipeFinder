namespace RecipeFinder.API.DTOs.Response.User
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Nickname { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
