
using Microsoft.EntityFrameworkCore;
using User_managment_system.Models;
using User_managment_system.ViewModels;

namespace User_managment_system.Repositories.UserTask
{
    public class TaskRepo(AppDbContext context) : ITaskRepo
    {
        private readonly AppDbContext _context = context;
        public void CreateTask(UserTaskSet task)
        {
            _context.Tasks.Add(task.ToTask());
            _context.SaveChanges();
        }

        //fire-and-forget trial
        public void DeleteTask(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
        }

        public async Task<List<Models.UserTask>> GetAllTasks()
        {
            return await _context.Tasks.ToListAsync() ?? [];
        }

        public async Task<Models.UserTask?> GetTaskById(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if(task != null)
            {
                return task;
            }
            return null;
        }

        public async Task UpdateTask(int id,  UserTaskSet task)
        {
            var myTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if(myTask != null)
            {
                myTask.Description = task.Description;
                myTask.Title = task.Title; ;
            }
            
        }
    }
}
