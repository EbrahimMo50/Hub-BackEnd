using Microsoft.EntityFrameworkCore;
using User_managment_system.Models;
using User_managment_system.ViewModels;

namespace User_managment_system.Repositories.User
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;
        private readonly Auth _auth;

        public UserRepo(AppDbContext context, Auth auth)
        {
            _context = context;
            _auth = auth;
        }

        public void CreateGroup(GroupSet group)
        {
            _context.Groups.Add(group.ToGroup());
            _context.SaveChanges();
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _context.Users.Include(x => x.Group).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.Password != password)
                return "404";

            return _auth.GenerateToken(user);
        }

        public string Register(UserSet user)
        {
            try
            {
                var User = user.ToUser();
                _context.Users.Add(User);
                _context.SaveChanges();
                return "200";
            }
            catch (Exception ex) 
            { 
                return ex.Message;
            }
        }

        public void UpdateUserGroup(int userId, int groupId)
        {
            var user = _context.Users.Include(x => x.Group).FirstOrDefault(x => x.Id == userId);
            var group = _context.Groups.Include(x => x.Users).FirstOrDefault(x => x.Id == groupId);
            if (user != null && group != null)
            {
                user.Group = group;
                group.Users.Add(user);
                _context.SaveChanges();
            }
        }

        public async Task<List<UserGet>> GetUsers()
        {
            var users = await _context.Users.Include(x => x.Group).ToListAsync() ?? []; 
            var userDto = new List<UserGet>();
            foreach (var user in users)
            {
                userDto.Add(UserGet.Transform(user));
            }
            return userDto ?? [];
        }

        public async Task<List<Group>> GetGroups()
        {
            return await _context.Groups.Include(x => x.Users).ToListAsync() ?? [];
        }
    }
}
