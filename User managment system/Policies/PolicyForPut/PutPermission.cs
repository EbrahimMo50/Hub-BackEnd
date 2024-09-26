using Microsoft.AspNetCore.Authorization;

namespace User_managment_system.Policies
{
    public class PutPermission : IAuthorizationRequirement
    {
        public PutPermission() { }
    }
}
