using User_managment_system.Models;
using User_managment_system.ViewModels;

namespace User_managment_system.Repositories.UserTask
{
    public interface ITaskRepo
    {
        public Task<List<Models.UserTask>> GetAllTasks();
        public Task<Models.UserTask> GetTaskById(int id);
        public Task CreateTask(UserTaskSet task);
        public Task UpdateTask(int id, UserTaskSet task);
        public Task DeleteTask(int id);
    }
}
