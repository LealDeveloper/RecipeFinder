using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinder.Application.Users.UpdateUser
{
    public class UpdateUserCommand
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
