using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz.Application.Templates.Commands;
using Quiz.Application.Templates.Queries;
using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Api.Controllers;

[Route("templates")]
[ApiController]
[Authorize(Roles = "admin,user")]
[Produces("application/json")]
public class TemplateController(IMediator mediator, ILogger<AdminController> logger) : ControllerBase
{
        /// <summary>
        /// Retrieve a list of templates with optional filtering and pagination.
        /// </summary>
        /// <remarks>
        /// <exclude>
        /// <strong>Example API Call:</strong>
        /// <code>GET /templates?page=1&amp;pageSize=10</code> <br/><br/>
        /// <strong>Query Parameters:</strong> <br/>
        /// <ul>
        ///     <li><strong>search</strong> (optional) → Filters templates by name or description.</li>
        ///     <li><strong>page</strong> (optional) → Specifies the page number for pagination.</li>
        ///     <li><strong>pageSize</strong> (optional) → Specifies the number of templates per page.</li>
        /// </ul>
        /// <br/>
        /// <strong>Response Status Codes:</strong> <br/>
        /// <ul>
        ///     <li><strong>200 OK</strong> → Templates successfully retrieved.</li>
        ///     <li><strong>500 Internal Server Error</strong> → Unexpected error occurred.</li>
        /// </ul>
        /// </exclude>
        /// </remarks>
        /// <param name="query">Query parameters for filtering and pagination.</param>
        /// <returns>Returns an HTTP response with the list of templates.</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginationResult<Template>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTemplates([FromQuery] GetTemplatesQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }
        
        /// <summary>
        /// Retrieve a template by its unique identifier.
        /// </summary>
        /// <remarks>
        /// <exclude>
        /// <strong>Example API Call:</strong>
        /// <code>GET /templates/{id}</code> <br/><br/>
        /// <strong>Response Status Codes:</strong> <br/>
        /// <ul>
        ///     <li><strong>200 OK</strong> → Template successfully retrieved.</li>
        ///     <li><strong>400 Bad Request</strong> → Invalid template ID.</li>
        ///     <li><strong>404 Not Found</strong> → Template not found.</li>
        ///     <li><strong>500 Internal Server Error</strong> → Unexpected error occurred.</li>
        /// </ul>
        /// </exclude>
        /// </remarks>
        /// <param name="id">The unique identifier of the template.</param>
        /// <returns>Returns an HTTP response with the template details or an error message.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Template), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTemplateById([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest(id);
            
            var query = new GetTemplateByIdQuery { TemplateId = id };
            
            var result = await mediator.Send(query);
            
            if (result.Success) return Ok(result);
            
            logger.LogError(result.Message, result.Errors);
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
        /// <summary>
        /// Create a new template.
        /// </summary>
        /// <remarks>
        /// <exclude>
        /// <strong>Example API Call:</strong>
        /// <code>POST /templates</code> <br/><br/>
        /// <strong>Response Status Codes:</strong> <br/>
        /// <ul>
        ///     <li><strong>200 OK</strong> → Template successfully created.</li>
        ///     <li><strong>400 Bad Request</strong> → Invalid request parameters.</li>
        ///     <li><strong>500 Internal Server Error</strong> → Unexpected error occurred.</li>
        /// </ul>
        /// </exclude>
        /// </remarks>
        /// <param name="command">The command containing the template data.</param>
        /// <returns>Returns an HTTP response indicating the result of the operation.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTemplate([FromBody] CreateTemplateCommand command)
        {
            var result = await mediator.Send(command);
            
            if (result.Success) return Ok();
            
            logger.LogError(result.Message, result.Errors);
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
        /// <summary>
        /// Update an existing template by its unique identifier.
        /// </summary>
        /// <remarks>
        /// <exclude>
        /// <strong>Example API Call:</strong>
        /// <code>PUT /templates/{id}</code> <br/><br/>
        /// <br/>
        /// <strong>Response Status Codes:</strong> <br/>
        /// <ul>
        ///     <li><strong>200 OK</strong> → Template successfully updated.</li>
        ///     <li><strong>400 Bad Request</strong> → Invalid template ID or request body.</li>
        ///     <li><strong>404 Not Found</strong> → Template not found.</li>
        ///     <li><strong>500 Internal Server Error</strong> → Unexpected error occurred.</li>
        /// </ul>
        /// </exclude>
        /// </remarks>
        /// <param name="id">The unique identifier of the template to be updated.</param>
        /// <param name="command">The command containing the updated template data.</param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTemplate([FromRoute] string id, [FromBody] UpdateTemplateCommand command)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest(id);

            command.TemplateId = id;
            
            var result = await mediator.Send(command);
            
            if (result.Success) return Ok();
            
            logger.LogError(result.Message, result.Errors);
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
        /// <summary>
        /// Delete a template by its unique identifier.
        /// </summary>
        /// <remarks>
        /// <exclude>
        /// <strong>Example API Call:</strong>
        /// <code>DELETE /templates/{id}</code> <br/><br/>
        /// <strong>Response Status Codes:</strong> <br/>
        /// <ul>
        ///     <li><strong>200 OK</strong> → Template successfully deleted.</li>
        ///     <li><strong>400 Bad Request</strong> → Invalid template ID.</li>
        ///     <li><strong>404 Not Found</strong> → Template not found.</li>
        ///     <li><strong>500 Internal Server Error</strong> → Unexpected error occurred.</li>
        /// </ul>
        /// </exclude>
        /// </remarks>
        /// <param name="id">The unique identifier of the template to be deleted.</param>
        /// <returns>Returns an HTTP response indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTemplate([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest(id);
            
            var command = new DeleteTemplateCommand { TemplateId = id };
            
            var result = await mediator.Send(command);
            
            if (result.Success) return Ok();

            logger.LogError(result.Message, result.Errors);
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
        /// <summary>
        /// Retrieve submissions for a specific template by its unique identifier.
        /// </summary>
        /// <remarks>
        /// <exclude>
        /// <strong>Example API Call:</strong>
        /// <code>GET /templates/{id}/submissions</code> <br/><br/>
        /// <strong>Response Status Codes:</strong> <br/>
        /// <ul>
        ///     <li><strong>200 OK</strong> → Submissions successfully retrieved.</li>
        ///     <li><strong>400 Bad Request</strong> → Invalid template ID.</li>
        ///     <li><strong>404 Not Found</strong> → Template or submissions not found.</li>
        ///     <li><strong>500 Internal Server Error</strong> → Unexpected error occurred.</li>
        /// </ul>
        /// </exclude>
        /// </remarks>
        /// <param name="id">The unique identifier of the template.</param>
        /// <returns>Returns an HTTP response with the list of submissions or an error message.</returns>
        [HttpGet("{id}/submissions")]
        [ProducesResponseType(typeof(Submission), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTemplateSubmissions([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest(id);
            
            var query = new GetTemplateSubmissionsQuery { TemplateId = id };
            
            var result = await mediator.Send(query);
            
            if (result.Success) return Ok(result);
            
            logger.LogError(result.Message, result.Errors);
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
        /// <summary>
        /// Retrieve a list of popular templates.
        /// </summary>
        /// <remarks>
        /// <exclude>
        /// <strong>Example API Call:</strong>
        /// <code>GET /templates/popular</code> <br/><br/>
        /// <strong>Response Status Codes:</strong> <br/>
        /// <ul>
        ///     <li><strong>200 OK</strong> → Popular templates successfully retrieved.</li>
        ///     <li><strong>500 Internal Server Error</strong> → Unexpected error occurred.</li>
        /// </ul>
        /// </exclude>
        /// </remarks>
        /// <param name="query">The query parameters for retrieving popular templates.</param>
        /// <returns>Returns an HTTP response with the list of popular templates.</returns>
        [HttpGet("popular")]
        [ProducesResponseType(typeof(PaginationResult<Template>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> GetPopularTemplates(GetPopularTemplatesQuery query)
        {
            var result = await mediator.Send(query);
            
            return Ok(result);
        }
}