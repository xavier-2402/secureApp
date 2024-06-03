using ISTA.SecureApp.Models;
using ISTA.SecureApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISTA.SecureApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service; 

        public UserController(UserService service)
        {
            _service = service;
        }


        [HttpPost]
        [Route("add")]
        public IActionResult Add(User user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(user);
            }
            return Ok(_service.Add(user));
        }


        [HttpGet]
        [Route("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }
    }
}
