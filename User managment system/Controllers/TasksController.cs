using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using User_managment_system.Repositories.UserTask;
using User_managment_system.ViewModels;

namespace User_managment_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController(ITaskRepo repo) : ControllerBase
    {
        private readonly ITaskRepo _repo = repo;

        [HttpGet("GetTasks")]
        [Authorize(Policy = "GetPolicy")]
        public async Task<IActionResult> GetTasks()
        {
            return Ok(await _repo.GetAllTasks());
        }

        [HttpGet("GetTask{Id}")]
        [Authorize(Policy = "GetPolicy")]
        public async Task<IActionResult> GetTaskById(int Id)
        {
            var task = await _repo.GetTaskById(Id);
            if(task != null)
                return Ok(task);

            return NotFound("could not find user");
        }

        [HttpPost("CreateTask")]
        [Authorize(Policy = "PostPolicy")]
        public IActionResult CreateTask(UserTaskSet task)
        {
            _repo.CreateTask(task);
            return Ok();
        }

        [HttpPut("UpdateTask")]
        [Authorize(Policy = "PutPolicy")]
        public IActionResult UpdateTask(int Id, UserTaskSet task)
        {
            _repo.UpdateTask(Id, task);
            return Ok();
        }

        [HttpDelete("DeleteTask")]
        [Authorize(Policy = "DeletePolicy")]
        public IActionResult DeleteTask(int Id)
        {
            _repo.DeleteTask(Id);
            return Ok();
        }
    }
}
