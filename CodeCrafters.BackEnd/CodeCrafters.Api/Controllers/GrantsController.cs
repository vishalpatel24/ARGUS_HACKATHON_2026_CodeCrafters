using CodeCrafters.Application.Grants.Dtos;
using CodeCrafters.Application.Grants.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeCrafters.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GrantsController(IGrantService grantService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<GrantTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<GrantTypeDto>>> GetAllGrants(CancellationToken cancellationToken)
    {
        var result = await grantService.GetAllGrantsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GrantTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GrantTypeDto>> GetGrantById(Guid id, CancellationToken cancellationToken)
    {
        var result = await grantService.GetGrantByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
