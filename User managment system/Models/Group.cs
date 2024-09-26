namespace User_managment_system.Models
{
    public class Group
    {
        public int Id { get; }
        public string Name { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public List<string> Validations { get; set; } = new List<string>();
    }
}
