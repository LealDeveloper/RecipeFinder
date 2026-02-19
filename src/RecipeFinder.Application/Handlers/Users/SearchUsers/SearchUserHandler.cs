using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using RecipeFinder.Application.Common.Interfaces;
using RecipeFinder.Application.Common.Pagination.User;
using RecipeFinder.Domain.Entities;
using System.Text.Json;

namespace RecipeFinder.Application.Handlers.Users.SearchUsers
{
    public class SearchUserHandler : IRequestHandler<SearchUserCommand, (IEnumerable<User>, int)>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDistributedCache _cache;

        public SearchUserHandler(IUserRepository userRepository, IDistributedCache cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }

        public async Task<(IEnumerable<User>, int)> Handle(SearchUserCommand command, CancellationToken cancellationToken = default)
        {
            var cacheKey = BuildCacheKey(command.DisplayName, command.Page, command.PageSize);

            // Tenta buscar do cache
            var cached = await _cache.GetStringAsync(cacheKey,cancellationToken);
            if (cached != null)
            {
                var cachedObj = JsonSerializer.Deserialize<PagedUserCache>(cached)!;

                // Converte de volta para entidades do domínio.
                // Mantemos PasswordHash fora do cache; usamos string.Empty como placeholder.
                var usersFromCache = cachedObj.User.Select(r => new User(
                    r.Id,
                    r.Email,
                    r.DisplayName
                ));

                return (usersFromCache, cachedObj.TotalCount);
            }

            // Busca do repositório com paginação
            var (users, totalCount) = await _userRepository.GetByDisplayNamePagedAsync(
                command.DisplayName, command.Page, command.PageSize, cancellationToken);

            // Converte para DTOs antes de guardar no cache
            var cacheObj = new PagedUserCache
            {
                User = users.Select(r => new UserCache
                {
                    Id = r.Id,
                    DisplayName = r.DisplayName,
                    Email = r.Email
                }).ToList(),
                TotalCount = totalCount
            };

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(cacheObj),
                options,
                cancellationToken);

            return (users, totalCount);
        }

        private static string BuildCacheKey(string displayName, int page, int pageSize)
        {
            return $"search:{displayName}:page:{page}:size:{pageSize}";
        }
    }
}
