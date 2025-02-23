using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Quiz.Api.Controllers;

[Route("blob")]
[ApiController]
[Authorize(Roles = "admin,user")]
[Produces("application/json")]
public class StorageController : ControllerBase
{
    [HttpPost("upload")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("File is empty or missing");
        
         return Ok();
    }
}