// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Serilog;
//
// namespace Quiz.Api.Controllers;
//
// [Route("debug")]
// [Authorize(Roles = "admin")]
// [ApiController]
// public class DebugController(ILogger<DebugController> logger) : ControllerBase
// {
//     [HttpGet("headers")]
//     public IActionResult Headers()
//     {
//         Console.WriteLine("Проверяем заголовки запроса...");
//         foreach (var header in Request.Headers)
//         {
//             Console.WriteLine($"Header: {header.Key} = {header.Value}");
//         }
//         return Ok("Заголовки выведены в консоль");
//     }
//     
//     [HttpGet("kibana")]
//     public IActionResult TestKibana()
//     {
//         logger.LogError("I'm kibana.");
//         
//         return Ok();
//     }
//     
//     [Authorize(Roles = "admin")]
//     [HttpGet("auth")]
//     public IActionResult TestAuth()
//     {
//         var user = HttpContext.User;
//         
//         return Ok(new
//         {
//             Authenticated = user.Identity?.IsAuthenticated,
//             AuthenticationType = user.Identity?.AuthenticationType,
//             Claims = user.Claims.Select(c => new { c.Type, c.Value })
//         });
//     }
// }