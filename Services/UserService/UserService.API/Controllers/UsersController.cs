using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Users.Commands.CreateUser;
using UserService.Application.Features.Users.Commands.DeleteUser;
using UserService.Application.Features.Users.Commands.UpdatePassword;
using UserService.Application.Features.Users.Commands.UpdateUserProfile;
using UserService.Application.Features.Users.Queries.GetAllUser;
using UserService.Application.Features.Users.Queries.GetUserByEmail;
using UserService.Application.Features.Users.Queries.GetUserById;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userId = await _sender.Send(command, cancellationToken);
        
        return CreatedAtAction(nameof(GetUserById), new { id = userId }, new { Id = userId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<GetAllUserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAllUserQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetUserByIdDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetUserByIdQuery(id), cancellationToken);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("by-email")]
    [ProducesResponseType(typeof(GetUserByEmailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetUserByEmailQuery(email), cancellationToken);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPut("{id:guid}/profile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfile([FromRoute] Guid id, [FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        await _sender.Send(new UpdateUserProfileCommand(id, request.FirstName, request.LastName), cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:guid}/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePassword([FromRoute] Guid id, [FromBody] UpdatePasswordRequest request, CancellationToken cancellationToken)
    {
        await _sender.Send(new UpdatePasswordCommand(id, request.NewPassword), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteUserCommand(id), cancellationToken);
        return NoContent();
    }
}

public record UpdateProfileRequest(string FirstName, string LastName);
public record UpdatePasswordRequest(string NewPassword);

