using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RecipeFinder.Application.Common.Interfaces;
using RecipeFinder.Domain.Entities;

namespace RecipeFinder.Application.Handlers.Users.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, User>
    {
        private readonly IUserRepository _repository;

        public GetUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> Handle(GetUserQuery query, CancellationToken cancellationToken = default)
        {
            var user = await _repository.GetByIdAsync(query.Id, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            return user;
        }
    }
}
