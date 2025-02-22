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
    public async Task<IActionResult> SearchTemplates([FromQuery] SearchTemplatesQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }
}