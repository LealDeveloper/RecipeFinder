using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinder.Application.Users.DeleteUser
{
    public class DeleteUserCommand
    {
        public Guid Id { get; }
        public DeleteUserCommand(Guid id)
        {
            Id = id;
        }
    }
}
