using User_managment_system.Models;

namespace User_managment_system.ViewModels
{
    public class UserSet
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User ToUser()
        {
            return new User() { Name = this.Name, Email = this.Email , Password = this.Password , Group = null};
        }
    }

}
