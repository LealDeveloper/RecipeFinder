namespace RecipeFinder.Application.Queries.Users.SearchUsers
{
    public record SearchUsersQuery(string DisplayName, int Page = 1, int PageSize = 10);
}
