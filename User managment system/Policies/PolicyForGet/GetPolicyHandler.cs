using Microsoft.AspNetCore.Authorization;
using User_managment_system.Models;

namespace User_managment_system.Policies.PolicyForGet
{
    public class GetPolicyHandler(AppDbContext context) : AuthorizationHandler<GetPermission>
    {
        private readonly AppDbContext _context = context;
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GetPermission requirement)
        {
            var GroupClaims = context.User.Claims.FirstOrDefault(x => x.Type == "groupId");

            if (GroupClaims == null)
                return Task.CompletedTask;

            var groupId = int.Parse(GroupClaims.Value);

            var group = _context.Groups.FirstOrDefault(g => g.Id == groupId);

            if (group!.Validations.FirstOrDefault(x => x == "get") != null)  //should not trust the null forgicing operator since the payload could be faulty for any reason amd no groups will be found
            {
                context.Succeed(requirement);
                Console.WriteLine("User has permission get");
            }

            return Task.CompletedTask;
        }
    }
}
