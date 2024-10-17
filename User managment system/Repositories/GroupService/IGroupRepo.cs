using User_managment_system.Models;
using User_managment_system.ViewModels;

namespace User_managment_system.Repositories.GroupService
{
    public interface IGroupRepo
    {
        public Task<List<Group>> GetGroups();
        public Task<Group>? GetGroupById(int id);
        public void UpdateGroup(int id, GroupSet group);
        public void DeleteGroup(int id);
        public void CreateGroup(GroupSet group);
    }
}
