using Microsoft.AspNetCore.Authorization;

namespace User_managment_system.Policies.PolicyForGet
{
    public class GetPermission : IAuthorizationRequirement
    {
        public GetPermission() { }
    }
}
