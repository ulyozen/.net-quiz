using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Quiz.Api.Controllers;

[Route("templates")]
[ApiController]
[Authorize(Roles = "admin,user")]
[Produces("application/json")]
public class LikeController
{
    
}