using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quiz.Application.Common;
using Quiz.Application.Users.Commands;

namespace Quiz.Api.Controllers;

[ApiController]
[Route("auth")]
[Produces("application/json")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<ActionResult<AuthResponse>> Create([FromBody] Create command)
    {
        var result = await mediator.Send(command);
        
        return !result.Success ? BadRequest(result) : Created(nameof(Create), result);
    }
    
    [HttpPost("signin")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] Login command)
    {
        var result = await mediator.Send(command);

        return !result.Success ? Unauthorized(result) : Created(nameof(Create), result);
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshToken command)
    {
        var result = await mediator.Send(command);
        
        return Ok(result);
    }
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword command)
    {
        var result = await mediator.Send(command);
        
        return Ok(result);
    }
    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok();
    }
}