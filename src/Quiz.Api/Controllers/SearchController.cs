using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quiz.Application.Templates.Queries;

namespace Quiz.Api.Controllers;

[Route("search")]
[ApiController]
[Produces("application/json")]
public class SearchController(IMediator mediator) : ControllerBase
{
    [HttpGet("templates")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTemplates([FromBody] SearchTemplatesQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }
}