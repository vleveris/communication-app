using CommunicationApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceholderController : ControllerBase
    {
        private readonly IPlaceholderService _placeholderService;

        public PlaceholderController(IPlaceholderService placeholderService)
        {
            _placeholderService = placeholderService;
        }

        [HttpGet]
        public ActionResult<Dictionary<string, string>> GetPlaceholders()
        {
            return Ok(_placeholderService.GetAvailablePlaceholders());
        }
    }
}
