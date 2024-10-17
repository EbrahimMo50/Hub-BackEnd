using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_managment_system.Models;
using User_managment_system.Repositories.GroupService;
using User_managment_system.ViewModels;

namespace User_managment_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepo _repo;

        public GroupController(IGroupRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("GetAllGroups")]
        [Authorize(Policy = "getPolicy")]
        public async Task<IActionResult> GetGroups()
        {
            return Ok(await _repo.GetGroups());
        }

        [HttpGet("GetGroupById{id}")]
        [Authorize(Policy = "getPolicy")]
        public async Task<IActionResult> GetGroupById(int id)
        {
            var result = await _repo.GetGroupById(id);

            if(result != null)
                return Ok(result);
            else
                return BadRequest("no group found with that id");
        }

        [HttpPost("CreateGroup")]
        [Authorize(Policy = "postPolicy")]
        public IActionResult CreateGroup(GroupSet group)
        {
            _repo.CreateGroup(group);
            return Ok();
        }

        [HttpPut("UpdateGroup")]
        //[Authorize(Policy = "putPolicy")]
        public IActionResult UpdateGroup(int id,GroupSet group)
        {
            _repo.UpdateGroup(id,group);
            return Ok();
        }

        [HttpDelete("DeleteGroup")]
        [Authorize(Policy = "deletePolicy")]
        public IActionResult DeleteGroup(int id)
        {
            _repo.DeleteGroup(id);
            return Ok();
        }
    }
}
