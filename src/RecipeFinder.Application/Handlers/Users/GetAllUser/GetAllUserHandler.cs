using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using RecipeFinder.Application.Common.Interfaces;
using RecipeFinder.Application.Common.Pagination.User;
using RecipeFinder.Domain.Entities;
using System.Linq;
using System.Text.Json;

namespace RecipeFinder.Application.Handlers.Users.GetAllUser
{


    public class GetAllUserHandler : IRequestHandler<GetAllUsersQuery, (IEnumerable<User>, int)>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDistributedCache _cache;

        public GetAllUserHandler(IUserRepository userRepository, IDistributedCache cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }

        public async Task<(IEnumerable<User>, int)> Handle(GetAllUsersQuery request, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"all_users:page:{request.Page}:size:{request.PageSize}";

            var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (cached != null)
            {
                var cachedObj = JsonSerializer.Deserialize<PagedUserCache>(cached)!;
                // Converte de volta para entidades do domínio.
                // Nota: não armazenamos `PasswordHash` no cache por segurança; 
                // aqui criamos a entidade com password vazio (não utilizado pela API).
                var usersFromCache = cachedObj.User.Select(r => new User(
                    r.Id,
                    r.Email,
                    r.DisplayName         
                ));
                return (usersFromCache, cachedObj.TotalCount);
            }

            // Busca paginada no repositório
            (var users, var totalCount) = await _userRepository.GetPagedAsync(request.Page, request.PageSize, cancellationToken);

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
                options, cancellationToken);

            return (users, totalCount);
        }
    }
}
