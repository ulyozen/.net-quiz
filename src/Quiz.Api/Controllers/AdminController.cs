using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz.Api.DTOs;
using Quiz.Api.Mappers;
using Quiz.Application.Common;
using Quiz.Application.Users.Commands.AdminActions;
using Quiz.Application.Users.Dtos;
using Quiz.Application.Users.Queries;
using Quiz.Core.Common;

namespace Quiz.Api.Controllers;

[Route("admin")]
[ApiController]
[Authorize(Roles = "admin")]
[Produces("application/json")]
public class AdminController(IMediator mediator, ILogger<AdminController> logger, 
    IValidator<ChangeRoleCommand> validator) : ControllerBase
{
    /// <summary>
    /// Retrieves a paginated list of users.
    /// </summary>
    /// <remarks>
    /// <exclude>
    /// <strong>Example API Call:</strong>
    /// <code>GET /admin/users?page=1&amp;pageSize=10</code> <br/><br/>
    /// <strong>Response Status Codes:</strong> <br/>
    /// <ul>
    ///     <li><strong>200 OK</strong> → Returns a list of users.</li>
    ///     <li><strong>400 Bad Request</strong> → Invalid query.</li>
    /// </ul>
    /// </exclude>
    /// </remarks>
    /// <param name="query">GetUsersQuery</param>
    /// <returns>
    /// Returns a <see cref="PaginationResult{UsersResponse}"/> containing the paginated list of users.
    /// </returns>
    [HttpGet("users")]
    [ProducesResponseType(typeof(PaginationResult<UsersResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
    {
        var result = await mediator.Send(query);

        return Ok(result);
    }
    
    /// <summary>
    /// Block a user.
    /// </summary>
    /// <remarks>
    /// <exclude>
    /// <strong>Example API Call:</strong>
    /// <code>POST /admin/user/{id}/ban</code> <br/><br/>
    /// <strong>Response Status Codes:</strong> <br/>
    /// <ul>
    ///     <li><strong>200 OK</strong> → User successfully blocked.</li>
    ///     <li><strong>400 Bad Request</strong> → Invalid user ID or user is already blocked.</li>
    ///     <li><strong>404 Not Found</strong> → User not found.</li>
    ///     <li><strong>500 Internal Server Error</strong> → Unexpected error occurred.</li>
    /// </ul>
    /// </exclude>
    /// </remarks>
    /// <param name="id">The unique identifier of the user to block.</param>
    /// <returns>Returns an HTTP response indicating the result of the operation.</returns>
    [HttpPost("user/{id}/ban")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> BlockUser([FromRoute] string id)
    {
        if (string.IsNullOrEmpty(id)) 
            return BadRequest(DomainErrors.Auth.RequestIdRequired);
        
        var result = await mediator.Send(new BlockCommand { UserId = id });
        
        if (result.Success) return Ok();

        if (result.Errors.Contains(DomainErrors.User.UserNotFound))
            return NotFound(result.Errors);
        
        if (result.Errors.Contains(DomainErrors.User.UserAlreadyBlocked))
            return BadRequest(result.Errors);
        
        logger.LogError(result.Message);
        
        return StatusCode(StatusCodes.Status500InternalServerError); 
    }
    
    /// <summary>
    /// Unblock a previously blocked user.
    /// </summary>
    /// <remarks>
    /// <exclude>
    /// <strong>Example API Call:</strong>
    /// <code>POST /admin/user/{id}/unban</code> <br/><br/>
    /// <strong>Response Status Codes:</strong> <br/>
    /// <ul>
    ///     <li><strong>200 OK</strong> → User successfully unblocked.</li>
    ///     <li><strong>400 Bad Request</strong> → Invalid user ID or user is already unblocked.</li>
    ///     <li><strong>404 Not Found</strong> → User not found.</li>
    ///     <li><strong>500 Internal Server Error</strong> → Unexpected error occurred.</li>
    /// </ul>
    /// </exclude>
    /// </remarks>
    /// <param name="id">The unique identifier of the user to unblock.</param>
    /// <returns>Returns an HTTP response indicating the result of the operation.</returns>
    [HttpPost("user/{id}/unban")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UnblockUser([FromRoute] string id)
    {
        if (string.IsNullOrEmpty(id)) 
            return BadRequest(DomainErrors.Auth.RequestIdRequired);
        
        var result = await mediator.Send(new UnblockCommand { UserId = id });
        
        if (result.Success) return Ok();
        
        if (result.Errors.Contains(DomainErrors.User.UserNotFound))
            return NotFound(result.Errors);
        
        if (result.Errors.Contains(DomainErrors.User.UserAlreadyUnblocked))
            return BadRequest(result.Errors);
        
        logger.LogError(result.Message);
        
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
    
    /// <summary>
    /// Change the role of a user.
    /// </summary>
    /// <remarks>
    /// <exclude>
    /// <strong>Example API Call:</strong>
    /// <code>POST /admin/user/{id}/change-role</code> <br/><br/>
    /// <strong>Response Status Codes:</strong> <br/>
    /// <ul>
    ///     <li><strong>200 OK</strong> → User role successfully changed.</li>
    ///     <li><strong>400 Bad Request</strong> → Invalid user ID or user already has this role.</li>
    ///     <li><strong>404 Not Found</strong> → User or role not found.</li>
    ///     <li><strong>500 Internal Server Error</strong> → Unexpected error occurred.</li>
    /// </ul>
    /// </exclude>
    /// </remarks>
    /// <param name="id">The unique identifier of the user whose role is being changed.</param>
    /// <param name="dto">The request object containing the new role.</param>
    /// <returns>Returns an HTTP response indicating the result of the operation.</returns>
    [HttpPost("user/{id}/change-role")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangeRole([FromRoute] string id, [FromBody] ChangeRoleDto dto)
    {
        var command = new ChangeRoleCommand { UserId = id, Role = dto.Role };
        
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.MapToError());
        
        var result = await mediator.Send(command);
        
        if (result.Success) return Ok();
        
        if (result.Errors.Contains(DomainErrors.User.UserHasRole))
            return BadRequest(result.Errors);
        
        if (result.Errors.Contains(DomainErrors.User.UserNotFound) ||
            result.Errors.Contains(DomainErrors.User.RoleNotFound))
        {
            return NotFound(result.Errors);
        }
        
        logger.LogError(result.Message);
        
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
    
    /// <summary>
    /// Delete a user.
    /// </summary>
    /// <remarks>
    /// <exclude>
    /// <strong>Example API Call:</strong>
    /// <code>DELETE /admin/user/{id}</code> <br/><br/>
    /// <strong>Response Status Codes:</strong> <br/>
    /// <ul>
    ///     <li><strong>200 OK</strong> → User successfully deleted.</li>
    ///     <li><strong>400 Bad Request</strong> → Invalid user ID.</li>
    ///     <li><strong>404 Not Found</strong> → User not found.</li>
    ///     <li><strong>500 Internal Server Error</strong> → Unexpected error occurred.</li>
    /// </ul>
    /// </exclude>
    /// </remarks>
    /// <param name="id">The unique identifier of the user to be deleted.</param>
    /// <returns>Returns an HTTP response indicating the result of the operation.</returns>
    [HttpDelete("user/{id}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser([FromRoute] string id)
    {
        if (string.IsNullOrEmpty(id)) 
            return BadRequest(DomainErrors.Auth.RequestIdRequired);
        
        var result = await mediator.Send(new DeleteCommand { UserId = id });
        
        if (result.Success) return Ok();
        
        if (result.Errors.Contains(DomainErrors.User.UserNotFound))
            return NotFound(result.Errors);
        
        logger.LogError(result.Message);
        
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}