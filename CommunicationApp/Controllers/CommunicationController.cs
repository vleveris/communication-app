using CommunicationApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationController : ControllerBase
    {
        private readonly ICommunicationService _communicationService;

        public CommunicationController(ICommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(int customerId, int templateId)
        {
            try
            {
                await _communicationService.SendMessageAsync(customerId, templateId);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
catch (Exception)
{
return BadRequest();
}
        }
    }
}
