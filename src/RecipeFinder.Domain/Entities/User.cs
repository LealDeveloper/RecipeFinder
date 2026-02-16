namespace RecipeFinder.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string DisplayName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    private User() { } // EF Core

    public User(Guid id, string email, string displayName, string passwordHash)
    {
        Id = id;
        DisplayName = displayName;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }
    public User(Guid id, string email, string displayName)
    {
        Id = id;
        DisplayName = displayName;
        Email = email;
    }
    public void UpdateNickname(string displayName)
    {
        DisplayName = displayName;
    }
    public void UpdateEmail(string email)
    {
        Email = email;
    }
    public void UpdateProfile(string displayName, string email)
    {
        DisplayName = displayName;
        Email = email;
    }
    public void UpdateProfile(string displayName, string email, string passwordHash)
    {
        DisplayName = displayName;
        Email = email;
        PasswordHash = passwordHash;
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }
}