using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecipeFinder.API.DTOs.Request.User;
using RecipeFinder.API.DTOs.Response.Generic;
using RecipeFinder.API.DTOs.Response.User;
using RecipeFinder.Application.Commands.Users.CreateUser;
using RecipeFinder.Domain.Entities;

namespace RecipeFinder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, 
                                            CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            new CreateUserCommand(
                request.Nickname,
                request.Email,
                request.Password),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById), 
            new { id }, 
            new { Id = id });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, 
                                            [FromQuery] int pageSize = 10, 
                                            CancellationToken cancellationToken = default)
    {
        var (users, totalCount) = await _sender.Send(new GetAllUsersQuery(page, pageSize), cancellationToken);
        var items = users.Select(Map).ToList();
        var response = new PagedResponse<UserResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, 
                                             CancellationToken cancellationToken = default)
    {
        var user = await _sender.Send(new GetUserQuery(id), cancellationToken);
        if (user == null) return NotFound();
        return Ok(Map(user));
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? nickname, 
                                            [FromQuery] int page = 1, 
                                            [FromQuery] int pageSize = 10, 
                                            CancellationToken cancellationToken = default)
    {
        var command = new SearchUserCommand(nickname ?? string.Empty, string.Empty, page, pageSize);
        var (users, totalCount) = await _sender.Send(command, cancellationToken);
        var items = users.Select(Map).ToList();
        var response = new PagedResponse<UserResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, 
                                            [FromBody] UpdateUserRequest request, 
                                            CancellationToken cancellationToken = default)
    {
        await _sender.Send(new UpdateUserCommand(id, request.Nickname, request.Email), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, 
                                            CancellationToken cancellationToken = default)
    {
        await _sender.Send(new DeleteUserCommand(id), cancellationToken);
        return NoContent();
    }

    private static UserResponse Map(User u) => new()
    {
        Id = u.Id,
        Nickname = u.DisplayName,
        Email = u.Email
    };
}
