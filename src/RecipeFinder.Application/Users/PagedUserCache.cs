using RecipeFinder.Application.Recipes;
using RecipeFinder.Application.Users;
using RecipeFinder.Domain.Entities;

public class PagedUserCache
{
    public List<UserCache> User { get; set; } = new();
    public int TotalCount { get; set; }
}
