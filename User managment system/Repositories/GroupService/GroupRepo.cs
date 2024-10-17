using Microsoft.EntityFrameworkCore;
using User_managment_system.Models;
using User_managment_system.ViewModels;

namespace User_managment_system.Repositories.GroupService
{
    public class GroupRepo(AppDbContext appContext) : IGroupRepo
    {
        private readonly AppDbContext _context = appContext;

        public void CreateGroup(GroupSet group)
        {
            _context.Groups.Add(group.ToGroup());
            _context.SaveChanges();

            //this is the real group we will add the users to (mass insertion)
            var physicalGroup = _context.Groups.FirstOrDefault(g => g.Name == group.Name && g.Validations == g.Validations);

            foreach (var id in group.UsersId)
            {
                var user = _context.Users.Include(x => x.Group).FirstOrDefault(x => x.Id == id);
                if (physicalGroup != null && user != null)
                {
                    user.Group = physicalGroup;
                    physicalGroup.Users.Add(user);
                }
            }
            _context.SaveChanges();
        }

        public void DeleteGroup(int id)
        {
            var group = _context.Groups.FirstOrDefault(x => x.Id == id);

            if(group != null)
                _context.Groups.Remove(group);
            _context.SaveChanges();
        }

        public async Task<Group>? GetGroupById(int id)
        {
            return await _context.Groups.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Group>> GetGroups()
        {
            return await _context.Groups.Include(x => x.Users).ToListAsync() ?? [];
        }

        public void UpdateGroup(int id,GroupSet group)
        {
            //to update a group we will add the new users whom entered group to the myGroup (the physical group)
            var myGroup = _context.Groups.Include(x => x.Users).FirstOrDefault(x => x.Id == id);

            if (myGroup != null) 
            {   
                //delete the users group value whom their group got changed
                var UsersIdRemovedFromGroup = myGroup.Users.Select(x => x.Id).Except(group.UsersId);

                foreach (var userid in UsersIdRemovedFromGroup)
                {
                    var user = _context.Users.FirstOrDefault(x => x.Id == userid);

                    if (user != null)
                        user.GroupId = null;
                }

                foreach(var userId in group.UsersId)
                {
                    //assign the users group to the new group
                    var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                    
                    if(user!=null)
                        user.GroupId = myGroup.Id;
                    _context.SaveChanges();
                }

                //set the group users reseting the array then adding the new users
                myGroup.Users.Clear();

                _context.SaveChanges();
                foreach (var userId in group.UsersId)
                {
                    var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                    if(user != null)
                    {
                        myGroup.Users.Add(user);
                    }
                }
                myGroup.Name = group.Name;
                myGroup.Validations = group.Validations;
                _context.SaveChanges();
            }
        }
    }
}
