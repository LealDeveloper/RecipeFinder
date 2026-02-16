using RecipeFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinder.Application.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByDisplayNameAsync(string displayname);
        Task<User?> UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<(List<User> Users, int TotalCount)> GetPagedAsync(int page, int pageSize);
        Task<(List<User> Users, int TotalCount)> GetByDisplayNamePagedAsync(string displayName, int page, int pageSize);
    }
}
