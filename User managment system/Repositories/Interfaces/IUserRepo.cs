using User_managment_system.ViewModels;

namespace User_managment_system.Repositories.Interfaces
{
    public interface IUserRepo
    {
        public Task<string> Login(string email, string password);
        public void Register(UserSet user);
        
    }
}
