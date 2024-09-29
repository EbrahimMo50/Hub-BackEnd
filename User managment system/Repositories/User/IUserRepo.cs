using User_managment_system.ViewModels;

namespace User_managment_system.Repositories.User
{
    public interface IUserRepo
    {
        public Task<string> Login(string email, string password);
        public void Register(UserSet user);
        public void UpdateUserGroup(int userId, int groupId);
        public void CreateGroup(GroupSet group);
        public Task<List<Models.User>> GetUsers();
        public Task<List<Models.Group>> GetGroups();
    }
}
