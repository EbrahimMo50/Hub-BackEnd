using Microsoft.EntityFrameworkCore;

namespace User_managment_system.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? GroupId { get; set; }
        public Group Group { get; set; } 
    }
}