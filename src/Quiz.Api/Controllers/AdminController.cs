using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz.Application.Users.Commands.AdminActions;
using Quiz.Application.Users.Queries;

namespace Quiz.Api.Controllers;

[Route("admin")]
[ApiController]
[Authorize(Roles = "admin")]
[Produces("application/json")]
public class AdminController(IMediator mediator) : ControllerBase
{
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost("user/{id}/ban")]
    public async Task<IActionResult> BlockUser(string id)
    {
        var command = new BlockCommand { UserId = id };
        var result = await mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("user/{id}/unban")]
    public async Task<IActionResult> UnblockUser(string id)
    {
        var command = new UnblockCommand { UserId = id };
        var result = await mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("user/{id}/change-role")]
    public async Task<IActionResult> ChangeRole(string id, ChangeRoleCommand request)
    {
        var command = new ChangeRoleCommand { UserId = id, Role = request.Role };
        var result = await mediator.Send(command);
        return Ok(result);
    }
    
    [HttpDelete("user/{id:guid}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var command = new DeleteCommand { UserId = id };
        await mediator.Send(command);
        return Ok();
    }
}