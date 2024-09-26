using Microsoft.EntityFrameworkCore;
using User_managment_system.Models;
using User_managment_system.Repositories.Interfaces;
using User_managment_system.ViewModels;

namespace User_managment_system.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;
        private readonly Auth _auth;

        public UserRepo()
        {
            _context = new AppDbContext();
            _auth = new Auth();
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _context.Users.Include(x => x.Group).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.Password != password)
                return "404";
            
            return _auth.GenerateToken(user);
        }

        public void Register(UserSet user)
        {
            var User = user.ToUser();
            _context.Add(User);
            _context.SaveChanges();
        }
    }
}
