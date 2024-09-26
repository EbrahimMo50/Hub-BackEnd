using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using User_managment_system.Models;

namespace User_managment_system.Policies.PolicyForPost
{
    public class PostPolicyHandler : AuthorizationHandler<PostPermission>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PostPermission requirement)
        {
            var GroupClaims = context.User.Claims.FirstOrDefault(x => x.Type == "groupId");

            if (GroupClaims == null)
                return Task.CompletedTask;

            var groupId = int.Parse(GroupClaims.Value);


            AppDbContext DbContext = new AppDbContext();

            var group = DbContext.Groups.FirstOrDefault(g => g.Id == groupId);

            if(group.Validations.FirstOrDefault(x => x == "post") != null)
            {
                context.Succeed(requirement);
                Console.WriteLine("User has permission post");
            }

            return Task.CompletedTask;
        }
    }
}
