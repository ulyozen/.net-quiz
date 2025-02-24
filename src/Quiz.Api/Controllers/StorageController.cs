using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz.Core.Common;

namespace Quiz.Api.Controllers;

[Route("blob")]
[ApiController]
[Authorize(Roles = "admin,user")]
[Produces("application/json")]
public class StorageController : ControllerBase
{
    /// <summary>
    /// Upload a file.
    /// </summary>
    /// <remarks>
    /// <exclude>
    /// <strong>Example API Call:</strong>
    /// <code>POST blob/upload</code> <br/><br/>
    /// <strong>Request:</strong> <br/>
    /// The request must include a file in the form-data body. <br/><br/>
    /// <strong>Response Status Codes:</strong> <br/>
    /// <ul>
    ///     <li><strong>200 OK</strong> → File successfully uploaded.</li>
    ///     <li><strong>400 Bad Request</strong> → The uploaded file is empty.</li>
    ///     <li><strong>500 Internal Server Error</strong> → Unexpected error occurred.</li>
    /// </ul>
    /// </exclude>
    /// </remarks>
    /// <param name="file">The file to be uploaded.</param>
    /// <returns>Returns an HTTP response indicating the result of the operation.</returns>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest(DomainErrors.BlobStorage.FileIsEmpty);
        
         return Ok();
    }
}