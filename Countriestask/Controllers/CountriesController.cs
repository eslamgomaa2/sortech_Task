using Countriestask.Dtos;
using Countriestask.Services.BlockCountry;
using Microsoft.AspNetCore.Mvc;

namespace Countriestask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly IBlockCountryService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CountriesController(IBlockCountryService service, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("add")]
        public IActionResult BlockCountry([FromBody] BlockCountryRequest request)
        {
            var result = _service.BlockCountry(request.CountryCode, request.DurationMinutes);
            
            return Ok (result);
        }

        [HttpDelete("Delete")]
        public IActionResult UnblockCountry(string countryCode)
        {
            var result = _service.RemoveCountry(countryCode);
            return Ok(result);
            
        }

        [HttpGet("GetblockedCountries")]
        public IActionResult GetBlocked([FromQuery] PaginationRequest pagination)
        {
            var result = _service.GetBlockedCountries(pagination.page, pagination.PageSize);
            return Ok(result);
        }

        [HttpGet("CheckIfIpBlocked")]
        public async Task<IActionResult> CheckIpBlockedAsync([FromQuery] string? ipAddress = null)
        {

            var response = await _service.CheckIpBlockedAsync(ipAddress);
            return Ok(response);
        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            var response = await _service.CheckIpBlockedAsync();
            return Ok(response);
        }
    }

}
