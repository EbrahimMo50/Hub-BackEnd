using Microsoft.AspNetCore.Authorization;

namespace User_managment_system.Policies.PolicyForPost
{
    public class PostPermission : IAuthorizationRequirement
    {
        public PostPermission() { }
    }
}
