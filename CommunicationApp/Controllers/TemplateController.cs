using CommunicationApp.Models;
using CommunicationApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _templateService;

        public TemplateController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Template>>> GetTemplates()
        {
            var templates = await _templateService.GetAllAsync();
            return Ok(templates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Template>> GetTemplate(int id)
        {
            var template = await _templateService.GetByIdAsync(id);

            if (template == null)
            {
                return NotFound();
            }

            return Ok(template);
        }

        [HttpPost]
        public async Task<ActionResult<Template>> CreateTemplate(Template template)
        {
            await _templateService.AddAsync(template);
            return CreatedAtAction(nameof(GetTemplate), new { id = template.Id }, template);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTemplate([FromRoute] int id, [FromBody] Template template)
        {
            if (id != template.Id)
            {
                return BadRequest();
            }

            var existingTemplate = await _templateService.GetByIdAsync(id);
            if (existingTemplate == null)
            {
                return NotFound();
            }

            await _templateService.UpdateAsync(template);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemplate([FromRoute] int id)
        {
            var template = await _templateService.GetByIdAsync(id);
            if (template == null)
            {
                return NotFound();
            }

            await _templateService.DeleteAsync(id);

            return NoContent();
        }
    }
}
