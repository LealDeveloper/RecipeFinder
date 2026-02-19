namespace RecipeFinder.Application.Common.Pagination.User
{
    public class PagedUserCache
    {
        public List<UserCache> User { get; set; } = new();
        public int TotalCount { get; set; }
    }
}

