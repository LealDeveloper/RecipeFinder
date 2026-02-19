using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinder.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        // Exemplo: última vez que fez login
        public DateTime LastLogin { get; set; }

        // Exemplo: flag de usuário ativo
        public bool IsActive { get; set; } = true;
    }

}
