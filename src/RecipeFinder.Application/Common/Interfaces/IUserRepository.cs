using RecipeFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RecipeFinder.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByDisplayNameAsync(string displayname, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteAsync(User user, CancellationToken cancellationToken = default);
        Task<(List<User> Users, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<(List<User> Users, int TotalCount)> GetByDisplayNamePagedAsync(string displayName, int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
