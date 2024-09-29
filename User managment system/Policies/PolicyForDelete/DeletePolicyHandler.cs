using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using User_managment_system.Models;

namespace User_managment_system.Policies
{
    public class DeletePolicyHandler(AppDbContext context) : AuthorizationHandler<DeletePermission>
    {   
        private readonly AppDbContext _context = context;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DeletePermission requirement)
        {
            var GroupClaims = context.User.Claims.FirstOrDefault(x => x.Type == "groupId");

            if (GroupClaims == null)
                return Task.CompletedTask;

            var groupId = int.Parse(GroupClaims.Value);

            var group = _context.Groups.FirstOrDefault(g => g.Id == groupId);

            if(group.Validations.FirstOrDefault(x => x == "delete") != null)
            {
                context.Succeed(requirement);
                Console.WriteLine("User has permission delete");
            }

            return Task.CompletedTask;
        }
    }
}
