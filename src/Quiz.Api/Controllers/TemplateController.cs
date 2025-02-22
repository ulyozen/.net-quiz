using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz.Application.Templates.Commands;
using Quiz.Application.Templates.Queries;

namespace Quiz.Api.Controllers;

[Route("templates")]
[ApiController]
[Produces("application/json")]
public class TemplateController(IMediator mediator, ILogger<AdminController> logger) : ControllerBase
{
        /// <summary>
        /// Получить список всех шаблонов (с пагинацией и фильтрацией).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetTemplates([FromQuery] GetTemplatesQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }
        
        /// <summary>
        /// Получить шаблон по ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTemplateById([FromRoute] GetTemplateByIdQuery id)
        {
            var query = new GetTemplateByIdQuery();
            
            var result = await mediator.Send(query);

            
            return Ok(result);
        }

        /// <summary>
        /// Создать новый шаблон.
        /// </summary>
        [Authorize] // Только авторизованные пользователи могут создавать шаблоны
        [HttpPost]
        public async Task<IActionResult> CreateTemplate([FromBody] CreateTemplateCommand command)
        {
            var result = await mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Обновить шаблон (только автор шаблона или админ).
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTemplate([FromRoute] string id, [FromBody] UpdateTemplateCommand command)
        {
            var result = await mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Удалить шаблон (только автор или админ).
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemplate([FromRoute] string id, [FromQuery] DeleteTemplateCommand command)
        {
            command.TemplateId = id;
            var result = await mediator.Send(command);

            return Ok();
        }

        /// <summary>
        /// Получить список всех заполненных форм для данного шаблона.
        /// </summary>
        [Authorize]
        [HttpGet("{id}/submissions")]
        public async Task<IActionResult> GetTemplateSubmissions([FromRoute] string id, GetTemplateSubmissionsQuery query)
        {
            var result = await mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Получить топ-5 популярных шаблонов (по количеству заполненных форм).
        /// </summary>
        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularTemplates(GetPopularTemplatesQuery query)
        {
            var result = await mediator.Send(query);

            return Ok(result);
        }
}