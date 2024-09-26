using User_managment_system.Models;

namespace User_managment_system.Data
{
    public static class InitDb
    {
        public static void Init()
        { 
            AppDbContext context = new AppDbContext();

            if (!context.Users.Any()) {             
                User admin = new User() { Name = "admin", Email = "a@gmail.com", Password = "1234"};

                Group admins = new Group() { Name = "Admins", Validations = new List<string> { "get", "put", "delete", "post" }, Users = new List<User> { admin } };

                admin.Group = admins;
           
                context.Users.Add(admin);
                context.Groups.Add(admins);
                context.SaveChanges();
            }
        }
    }
}
