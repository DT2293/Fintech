using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization
{
    public class FunctionRequirement : IAuthorizationRequirement
    {
        public string FunctionKey { get; }
        public FunctionRequirement(string functionKey)
        {
            FunctionKey = functionKey;
        }
    }

    public class FunctionAuthorizationHandler : AuthorizationHandler<FunctionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FunctionRequirement requirement)
        {
            var hasPermission = context.User.Claims.Any(c =>
                c.Type == "function" && c.Value == requirement.FunctionKey);

            if (hasPermission)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }


    public class HasFunctionAttribute : AuthorizeAttribute
    {
        public HasFunctionAttribute(string functionKey)
        {
            Policy = $"Function:{functionKey}";
        }
    }
}
