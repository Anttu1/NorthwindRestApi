using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;
using NorthwindRestApi.Services.Interfaces;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticateService _authenticateService;

        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        // Tähän tulee Front endin kirjautumisyritys
        [HttpPost]
        public ActionResult Post([FromBody] Credentials tunnukset)
        {
            var loggedUser = _authenticateService.Authenticate(tunnukset.Username, tunnukset.Password);

            if (loggedUser == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(loggedUser); // Palautus front endiin (sis. vain loggedUser luokan mukaiset kentät)
        }
    }
}
