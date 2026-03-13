using System.Security.Claims;
using CodeCrafters.Application.Auth.Dtos;
using CodeCrafters.Application.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeCrafters.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "PlatformAdmin")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponseDto>> CreateUser(
        [FromBody] CreateUserDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await userService.CreateUserAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Roles = "PlatformAdmin,ProgramOfficer")]
    [ProducesResponseType(typeof(List<UserResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserResponseDto>>> GetAllUsers(CancellationToken cancellationToken)
    {
        var result = await userService.GetAllUsersAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponseDto>> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentRole = User.FindFirst(ClaimTypes.Role)?.Value;

        if (currentRole != "PlatformAdmin" && currentUserId != id.ToString())
            return Forbid();

        var result = await userService.GetUserByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "PlatformAdmin")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponseDto>> UpdateUser(
        Guid id,
        [FromBody] UpdateUserDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await userService.UpdateUserAsync(id, dto, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}/deactivate")]
    [Authorize(Roles = "PlatformAdmin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateUser(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await userService.DeactivateUserAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
