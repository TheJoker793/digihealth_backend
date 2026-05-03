using Microsoft.AspNetCore.Mvc;

namespace Auth_microservice.Controllers
{
    [ApiController]
    public class WellKnownController : ControllerBase
    {
        // =========================
        // JWKS
        // =========================
        [HttpGet("/.well-known/jwks.json")]
        public IActionResult GetJwks()
        {
            return Ok(new { keys = new object[] { } });
        }

        // =========================
        // OPENID CONFIG
        // =========================
        [HttpGet("/openid-configuration")]
        public IActionResult OpenIdConfiguration()
        {
            return Ok(new
            {
                issuer = "DMI.Auth",
                jwks_uri = "/.well-known/jwks.json"
            });
        }
    }
}