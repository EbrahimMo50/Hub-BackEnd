using User_managment_system.Models;

namespace User_managment_system.ViewModels
{
    public class UserGet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? GroupId { get; set; }
        public string? GroupName { get; set; }
    
        public static UserGet Transform(User user)
        {
            return new UserGet() { Id = user.Id,
                Email = user.Email,
                GroupId = user.GroupId,
                GroupName = user.Group != null ? user.Group.Name : "No Group Assinged!",
                Name = user.Name,
                Password = user.Password };
        }
    }
    
}
