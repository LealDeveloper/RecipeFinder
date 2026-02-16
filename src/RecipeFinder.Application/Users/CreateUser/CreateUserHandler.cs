using RecipeFinder.Application.Interfaces;
using RecipeFinder.Application.Recipes.CreateRecipe;
using RecipeFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RecipeFinder.Application.Users.CreateUser
{
    public class CreateUserHandler
    {
        private readonly IUserRepository _userRepostiroy;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserHandler(IUserRepository userRepostiroy, IPasswordHasher passwordHasher)
        {
            _userRepostiroy = userRepostiroy;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(CreateUserCommand command)
        {
            var emailExists = await _userRepostiroy.GetByEmailAsync(command.Email);
            if (emailExists != null)
                throw new InvalidOperationException("Email already in use.");

            var nicknameExists = await _userRepostiroy.GetByDisplayNameAsync(command.DisyplayName);
            if (nicknameExists != null)
                throw new InvalidOperationException("Nickname already in use.");
            var hash = _passwordHasher.Hash(command.Password);

            var user = new User(
                Guid.NewGuid(),
                command.Email,
                command.DisyplayName,
                hash
            );

            await _userRepostiroy.AddAsync(user);
            return user.Id;
        }

    }
}
