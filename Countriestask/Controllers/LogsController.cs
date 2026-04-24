using Countriestask.Dtos;
using Countriestask.Services.BlockCountry;
using Microsoft.AspNetCore.Mvc;

namespace Countriestask.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class LogsController : ControllerBase
    {
        private readonly IBlockCountryService _service;

        public LogsController(IBlockCountryService service) => _service = service;

        [HttpGet("all attempts ")]
        public IActionResult GetAttempts([FromQuery] PaginationRequest pagination)
        {
            var result = _service.GetBlockedAttempts(pagination.page, pagination.PageSize);
            return Ok(result);
        }
    }
}
