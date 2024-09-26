using Microsoft.AspNetCore.Authorization;

namespace User_managment_system.Policies
{
    public class DeletePermission : IAuthorizationRequirement
    {
        public DeletePermission() { }
    }
}
