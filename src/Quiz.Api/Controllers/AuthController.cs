using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quiz.Application.Users.Commands;

namespace Quiz.Api.Controllers;

[ApiController]
[Route("auth")]
[Produces("application/json")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> Create([FromBody] Create command)
    {
        var result = await mediator.Send(command);
        if (!result.Success)
            return BadRequest(result);
        
        return Created(nameof(Create), result);
    }
    
    [HttpPost("signin")]
    public async Task<IActionResult> Login([FromBody] Login command)
    {
        var result = await mediator.Send(command);
        if (!result.Success)
            return Unauthorized(result);
            
        return Ok(result);
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