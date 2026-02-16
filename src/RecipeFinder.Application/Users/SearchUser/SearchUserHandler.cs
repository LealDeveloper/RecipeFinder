using Microsoft.Extensions.Caching.Distributed;
using RecipeFinder.Application.Interfaces;
using RecipeFinder.Domain.Entities;
using System.Text.Json;

namespace RecipeFinder.Application.Users.SearchUser
{
    // DTO simples para armazenar no cache


    public class SearchUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IDistributedCache _cache;

        public SearchUserHandler(IUserRepository userRepository, IDistributedCache cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }

        public async Task<(IEnumerable<User>, int)> Handle(SearchUserCommand command)
        {
            var cacheKey = BuildCacheKey(command.Email, command.Page, command.PageSize);

            // Tenta buscar do cache
            var cached = await _cache.GetStringAsync(cacheKey);
            if (cached != null)
            {
                var cachedObj = JsonSerializer.Deserialize<PagedUserCache>(cached)!;

                // Converte de volta para entidades do domínio
                var recipesFromCache = cachedObj.User.Select(r => new User(
                    r.Id,
                    r.DisplayName,
                    r.Email
                ));

                return (recipesFromCache, cachedObj.TotalCount);
            }

            // Busca do repositório com paginação
            var (users, totalCount) = await _userRepository.GetByDisplayNamePagedAsync(
                command.DisplayName, command.Page, command.PageSize);

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
                options);

            return (users, totalCount);
        }

        private static string BuildCacheKey(string displayName, int page, int pageSize)
        {
            return $"search:{displayName}:page:{page}:size:{pageSize}";
        }
    }
}
