using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.DataProtection;

namespace DataProtectionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProtectionController : Controller
    {
        private readonly IDataProtector _protector;

        public ProtectionController(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("DataProtectionApi.v1");
        }

        [HttpPost("protect")]
        public IActionResult Protect([FromBody] string value)
        {
            try
            {   if (string.IsNullOrEmpty(value))
                {
                    return BadRequest("Value cannot be null or empty.");
                }
                else
                {
                    var protectedValue = _protector.Protect(value);
                    return Ok(new
                    {
                        original = value,
                        protectedValue
                    });
                }
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while processing the request.");
            }
           
        }

        [HttpPost("unprotect")]
        public IActionResult Unprotect([FromBody] string protectedValue)
        {
            try
            {
                var originalValue = _protector.Unprotect(protectedValue);

                return Ok(new
                {
                    protectedValue,
                    original = originalValue
                });
            }
            catch (Exception)
            {
                return BadRequest("Invalid protected value.");
            }
        }

    }
}
