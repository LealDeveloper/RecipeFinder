namespace RecipeFinder.API.DTOs.Request.User
{
    public class UpdateUserRequest
    {
        public string Nickname { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
