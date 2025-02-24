using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quiz.Application.Users.Commands.AuthActions;
using Quiz.Application.Users.Dtos;
using Quiz.Core.Common;

namespace Quiz.Api.Controllers;

[ApiController]
[Route("auth")]
[Produces("application/json")]
public class AuthController(IMediator mediator, ILogger<AuthController> logger) : ControllerBase
{
    /// <summary>
    /// User registration
    /// </summary>
    /// <remarks>
    /// <exclude>
    /// Example API Call:
    /// <code>POST /auth/signup</code> <br/><br/>
    /// Response Status Codes: <br/><br/>
    /// <ul>
    ///     <li><strong>200 OK</strong> → Successful registration.</li>
    ///     <li><strong>200 Bad Request</strong> → Validation errors (e.g., invalid password format).</li>
    ///     <li><strong>409 Conflict</strong> → Email already exists.</li>
    /// </ul>
    /// </exclude>
    /// </remarks>
    /// <param name="command">SignUp</param>
    /// <returns>Returns success response or an error response</returns>
    [HttpPost("signup")]
    [ProducesResponseType(typeof(SuccessResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Signup([FromBody] SignUpCommand command)
    {
        var result = await mediator.Send(command);
        
        if (result is not ErrorResponse error) return Ok(result);
        
        if (error.Message.Contains(DomainErrors.Auth.EmailAlreadyExists)) 
            return Conflict(error);
        
        logger.LogError(error.Message);
        
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
    
    /// <summary>
    /// User authentication
    /// </summary>
    /// <remarks>
    /// <exclude>
    /// Example API Call:
    /// <code>POST /auth/signin</code> <br/><br/>
    /// Response Status Codes: <br/><br/>
    /// <ul>
    ///     <li><strong>200 OK</strong> → Successful login.</li>
    ///     <li><strong>401 Unauthorized</strong> → Incorrect email or password.</li>
    ///     <li><strong>403 Forbidden</strong> → User is blocked.</li>
    ///     <li><strong>400 Bad Request</strong> → Validation errors.</li>
    /// </ul>
    /// </exclude>
    /// </remarks>
    /// <param name="command">SignIn</param>
    /// <returns>Returns a JWT token or an error response</returns>
    [HttpPost("signin")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Signin([FromBody] SignInCommand command)
    {
        var result = await mediator.Send(command);
        
        if (result is not ErrorResponse error) return Ok(result);

        if (error.Message.Contains(DomainErrors.User.UserBlocked))
            return StatusCode(StatusCodes.Status403Forbidden, error);
        
        
        if (error.Message.Contains(DomainErrors.Auth.EmailNotFound) ||
            error.Message.Contains(DomainErrors.Auth.InvalidPassword))
            return Unauthorized(error);
        
        logger.LogError(error.Message);
        
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
    
    /// <summary>
    /// Refresh authentication token
    /// </summary>
    /// <remarks>
    /// <exclude>
    /// Example API Call:
    /// <code>POST /auth/refresh-token</code> <br/><br/>
    /// Response Status Codes: <br/><br/>
    /// <ul>
    ///     <li><strong>200 OK</strong> → Successfully refreshed the access token.</li>
    ///     <li><strong>400 Bad Request</strong> → Validation errors.</li>
    ///     <li><strong>401 Unauthorized</strong> → Invalid, expired, or missing refresh token.</li>
    /// </ul>
    /// </exclude>
    /// </remarks>
    /// <returns>Returns a new access token or an error response</returns>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken()
    {
        var command = new RefreshTokenCommand();
        
        var result = await mediator.Send(command);
        
        if (result is not ErrorResponse error) return Ok(result);

        if (error.Message.Contains(DomainErrors.Auth.RefreshTokenExpired) ||
            error.Message.Contains(DomainErrors.Auth.RefreshTokenMissing) ||
            error.Message.Contains(DomainErrors.Auth.RefreshTokenNotFound))
            return Unauthorized(error);
        
        return BadRequest(error);
    }
    
    /// <summary>
    /// Request password reset
    /// </summary>
    /// <remarks>
    /// <exclude>
    /// Example API Call:
    /// <code>POST /auth/forgot-password</code> <br/><br/>
    /// Response Status Codes:
    /// <ul>
    ///     <li><strong>200 OK</strong> → Password reset link sent successfully.</li>
    ///     <li><strong>400 Bad Request</strong> → Validation errors.</li>
    ///     <li><strong>401 Unauthorized</strong> → Email not found.</li>
    /// </ul>
    /// </exclude>
    /// </remarks>
    /// <param name="command">ForgotPassword</param>
    /// <returns>Returns a success response or an error response</returns>
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        var result = await mediator.Send(command);
        
        if (result is not ErrorResponse error) return Ok();
        
        if (error.Message.Contains(DomainErrors.Auth.EmailNotFound))
            return NotFound(error);
        
        return BadRequest(error);
    }
    
    /// <summary>
    /// Logout user
    /// </summary>
    /// <remarks>
    /// <exclude>
    /// Example API Call:
    /// <code>POST /auth/logout</code> <br/><br/>
    /// Response Status Codes:
    /// <ul>
    ///     <li><strong>200 OK</strong> → Successfully logged out.</li>
    /// </ul>
    /// </exclude>
    /// </remarks>
    /// <returns>Returns a success response</returns>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        var command = new LogoutCommand();
        
        await mediator.Send(command);
        
        return Ok();
    }
}