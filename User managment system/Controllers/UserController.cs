using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_managment_system.Repositories.Interfaces;
using User_managment_system.ViewModels;

namespace User_managment_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _repo;
        public UserController(IUserRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email,string password)
        {
            var result = await _repo.Login(email, password);  
            if(result == "404")
                return NotFound("user was not found");

            return Ok(result);
        }

        [HttpPost("Register")]
        public IActionResult Register(UserSet user)
        {
            _repo.Register(user);
            return Ok();
        }
        [HttpGet("test")]
        [Authorize(Policy = "getPolicy")]
        public IActionResult test()
        {
            return Ok();
        }
    }
}
