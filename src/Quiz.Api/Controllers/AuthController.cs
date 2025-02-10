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
        return Ok(await mediator.Send(command));
    }
    
    [HttpPost("signin")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] Login command)
    {
        return Ok(await mediator.Send(command));
    }
    
    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshToken command)
    {
        return Ok(await mediator.Send(command));
    }
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword command)
    {
        return Ok(await mediator.Send(command));
    }
    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok();
    }
}