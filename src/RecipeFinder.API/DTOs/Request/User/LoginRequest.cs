namespace RecipeFinder.API.DTOs.Request.User
{
    public class LoginRequest
    {
        public string EmailOrNickname { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
