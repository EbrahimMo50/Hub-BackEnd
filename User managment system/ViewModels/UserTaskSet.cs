using User_managment_system.Models;

namespace User_managment_system.ViewModels
{
    public class UserTaskSet
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public UserTask ToTask()
        {
            return new UserTask()
            {
                Title = this.Title,
                Description = this.Description
            };
        }
    }
}
