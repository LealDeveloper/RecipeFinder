using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinder.Application.Users.SearchUser
{
    public class SearchUserCommand
    {
        public string DisplayName { get; }
        public string Email { get; }
        public int Page { get; }
        public int PageSize { get; }

        public SearchUserCommand(string displayName, string email, int page = 1, int pageSize = 10)
        {
            DisplayName = displayName;
            Email = email;
            Page = page;
            PageSize = pageSize;
        }
    }
}
