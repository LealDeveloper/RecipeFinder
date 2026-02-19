using RecipeFinder.Domain.Entities.Common;

namespace RecipeFinder.Domain.Entities;

public class User : Entity<Guid>
{
    public string DisplayName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected User() { } // For EF

    public User(Guid id, string displayName, string email)
        : base(id)
    {
        DisplayName = displayName;
        Email = email;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(string displayName, string email)
    {
        DisplayName = displayName;
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }
}
