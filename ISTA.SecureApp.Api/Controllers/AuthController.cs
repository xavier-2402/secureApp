using ISTA.SecureApp.Models;
using ISTA.SecureApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISTA.SecureApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service; 

        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(Login login)
        {
            return Ok(_service.Login(login));
        }
    }
}
