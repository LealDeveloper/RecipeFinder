using Microsoft.Extensions.Caching.Distributed;
using RecipeFinder.Application.Interfaces;
using RecipeFinder.Domain.Entities;
using System.Linq;
using System.Text.Json;

namespace RecipeFinder.Application.Users.GetAllUser
{


    public class GetAllUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IDistributedCache _cache;

        public GetAllUserHandler(IUserRepository userRepository, IDistributedCache cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }

        public async Task<(IEnumerable<User>, int)> Handle(int page, int pageSize)
        {
            var cacheKey = $"all_users:page:{page}:size:{pageSize}";

            var cached = await _cache.GetStringAsync(cacheKey);
            if (cached != null)
            {
                var cachedObj = JsonSerializer.Deserialize<PagedUserCache>(cached)!;
                // Converte de volta para entidades do domínio
                var usersFromCache = cachedObj.User.Select(r => new User(
                    r.Id,
                    r.DisplayName,
                    r.Email
                ));
                return (usersFromCache, cachedObj.TotalCount);
            }

            // Busca paginada no repositório
            (var users, var totalCount) = await _userRepository.GetPagedAsync(page, pageSize);

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
    }
}
