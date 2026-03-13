using CodeCrafters.Application.Auth.Dtos;
using CodeCrafters.Application.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeCrafters.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponseDto>> Register(
        [FromBody] RegisterRequestDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await authService.RegisterAsync(dto, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginRequestDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await authService.LoginAsync(dto, cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
