using Microsoft.EntityFrameworkCore;
using RecipeFinder.Application.Interfaces;
using RecipeFinder.Domain.Entities;
using RecipeFinder.Infrastructure.Persistence;

namespace RecipeFinder.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RecipeDbContext _context;

    public UserRepository(RecipeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByDisplayNameAsync(string displayname)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.DisplayName == displayname);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<(List<User> Users, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var baseQuery = _context.Users.AsNoTracking();

        var totalCount = await baseQuery.CountAsync();

        var userIds = await baseQuery
            .OrderBy(u => u.DisplayName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => u.Id)
            .ToListAsync();

        var users = await _context.Users
            .AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();

        return (users, totalCount);
    }

    public async Task<(List<User> Users, int TotalCount)> GetByDisplayNamePagedAsync(string displayName, int page, int pageSize)
    {
        // Quando displayName for nulo ou vazio, retorna todos (paginados).
        var baseQuery = _context.Users
            .AsNoTracking()
            .Where(u => string.IsNullOrEmpty(displayName) || u.DisplayName.Contains(displayName));

        var totalCount = await baseQuery.CountAsync();

        var userIds = await baseQuery
            .OrderBy(u => u.DisplayName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => u.Id)
            .ToListAsync();

        var users = await _context.Users
            .AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();

        return (users, totalCount);
    }
}
