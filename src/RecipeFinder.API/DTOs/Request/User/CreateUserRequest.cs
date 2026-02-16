namespace RecipeFinder.API.DTOs.Request.User
{
    public class CreateUserRequest
    {
        public string Nickname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
