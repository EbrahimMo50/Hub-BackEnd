﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using User_managment_system.Models;

namespace User_managment_system.Policies.PolicyForPost
{
    public class PostPolicyHandler(AppDbContext context) : AuthorizationHandler<PostPermission>
    {
        private readonly AppDbContext _context = context;
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PostPermission requirement)
        {
            var GroupClaims = context.User.Claims.FirstOrDefault(x => x.Type == "groupId");

            if (GroupClaims == null || GroupClaims.Value == "")
                return Task.CompletedTask;

            var groupId = int.Parse(GroupClaims.Value);

            var group = _context.Groups.FirstOrDefault(g => g.Id == groupId);

            if(group.Validations.FirstOrDefault(x => x == "post") != null)
            {
                context.Succeed(requirement);
                Console.WriteLine("User has permission post");
            }

            return Task.CompletedTask;
        }
    }
}
