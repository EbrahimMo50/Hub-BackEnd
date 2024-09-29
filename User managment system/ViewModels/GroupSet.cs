using User_managment_system.Models;

namespace User_managment_system.ViewModels
{
    public class GroupSet
    {
        public string Name { get; set; }    
        public List<string> Validations { get; set; } = new List<string>();

        public Group ToGroup()
        {
            return new Group
            {
                Name = this.Name,
                Validations = this.Validations
            };
        }
    }
}
