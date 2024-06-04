using ISTA.SecureApp.Models;
using ISTA.SecureApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISTA.SecureApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _service; 

        public UserController(UserService service)
        {
            _service = service;
        }


        [HttpPost]
        [Route("add")]
        //[Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]

        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }
    }
}
