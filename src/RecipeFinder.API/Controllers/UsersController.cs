using Microsoft.AspNetCore.Mvc;
using RecipeFinder.Application.Users.CreateUser;
using RecipeFinder.Application.Interfaces;
using RecipeFinder.API.DTOs.Request.User;
using RecipeFinder.API.DTOs.Response.User;

namespace RecipeFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CreateUserHandler _createUserHandler;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UsersController(
            CreateUserHandler createUserHandler,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _createUserHandler = createUserHandler;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var id = await _createUserHandler.Handle(new CreateUserCommand(request.Nickname, request.Email, request.Password));
            return CreatedAtAction(nameof(GetById), new { id }, new { Id = id });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            var response = new UserResponse
            {
                Id = user.Id,
                Nickname = user.DisplayName,
                Email = user.Email
            };

            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? nickname)
        {
            if (string.IsNullOrWhiteSpace(nickname)) return BadRequest("nickname is required");

            var user = await _userRepository.GetByDisplayNameAsync(nickname);
            if (user == null) return NotFound();

            var response = new UserResponse
            {
                Id = user.Id,
                Nickname = user.DisplayName,
                Email = user.Email
            };

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // tenta por email primeiro, depois por nickname
            var user = await _userRepository.GetByEmailAsync(request.EmailOrNickname)
                       ?? await _userRepository.GetByDisplayNameAsync(request.EmailOrNickname);

            if (user == null) return Unauthorized("Invalid credentials");

            var valid = _passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!valid) return Unauthorized("Invalid credentials");

            // Aqui só devolvemos dados simples; em um cenário real, gerar um JWT ou session.
            var response = new UserResponse
            {
                Id = user.Id,
                Nickname = user.DisplayName,
                Email = user.Email
            };

            return Ok(response);
        }
    }
}
