using Microsoft.EntityFrameworkCore;
using RecipeFinder.Application.Common.Interfaces;
using RecipeFinder.Domain.Entities;
using RecipeFinder.Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace RecipeFinder.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RecipeDbContext _context;

    public UserRepository(RecipeDbContext context)
    {
        _context = context;
    }
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DomainUsers
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.DomainUsers.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.DomainUsers.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.DomainUsers.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetByDisplayNameAsync(string displayname, CancellationToken cancellationToken = default)
    {
        return await _context.DomainUsers
            .FirstOrDefaultAsync(u => u.DisplayName == displayname, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.DomainUsers
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<(List<User> Users, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var baseQuery = _context.DomainUsers.AsNoTracking();

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var userIds = await baseQuery
            .OrderBy(u => u.DisplayName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => u.Id)
            .ToListAsync(cancellationToken);

        var users = await _context.DomainUsers
            .AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync(cancellationToken);

        return (users, totalCount);
    }

    public async Task<(List<User> Users, int TotalCount)> GetByDisplayNamePagedAsync(string displayName, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var baseQuery = _context.DomainUsers
            .AsNoTracking()
            .Where(u => string.IsNullOrEmpty(displayName) || EF.Functions.ILike(u.DisplayName, $"%{displayName}%"));

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var userIds = await baseQuery
            .OrderBy(u => u.DisplayName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => u.Id)
            .ToListAsync(cancellationToken);

        var users = await _context.DomainUsers
            .AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync(cancellationToken);

        return (users, totalCount);
    }
}
