using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_managment_system.Repositories.User;
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
        public async Task<IActionResult> Login(LoginDto user)           //created a login dto as a test to see if single value types are looked for in the header or not there is a problem 415 with postman/front
        {
            var result = await _repo.Login(user.Email, user.Password);  
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

        [HttpPut("UpdateUserGroup")]
        [Authorize(Policy = "PutPolicy")]
        public IActionResult UpdateUserGroup(int UserId,int GroupId)
        {
            _repo.UpdateUserGroup(UserId,GroupId);
            return Ok();
        }

        [HttpPost("CreateGroup")]
        [Authorize(Policy = "PostPolicy")]
        public IActionResult CreateGroup(GroupSet group)
        {
            _repo.CreateGroup(group);
            return Ok();
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _repo.GetUsers());
        }

        [HttpGet("GetGroups")]
        public async Task<IActionResult> GetGroups()
        {
            return Ok(await _repo.GetGroups());
        }

    }
}
