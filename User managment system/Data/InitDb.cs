using Microsoft.EntityFrameworkCore;
using User_managment_system.Models;

namespace User_managment_system.Data
{
    public class InitDb(AppDbContext context)
    {
        private readonly AppDbContext _context = context;
        public void Init()
        { 
            if(!_context.Database.GetMigrations().Any())
            {
                _context.Database.Migrate();
            }

            if (!_context.Users.Any()) {

                User admin = new User() { Name = "admin", Email = "a@gmail.com", Password = "1234"};

                Group admins = new Group() { Name = "Admins", Validations = new List<string> { "get", "put", "delete", "post" }, Users = new List<User> { admin } };

                admin.Group = admins;
           
                _context.Users.Add(admin);
                _context.Groups.Add(admins);
                _context.SaveChanges();
            }
            
        }
    }
}
