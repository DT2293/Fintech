//using System.Security.Claims;
//using ManageUserSystem.Common;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Controllers;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.EntityFrameworkCore;

//namespace ManageUserSystem.Filter
//{
//    public class HasPermissionAttribute : Attribute, IAuthorizationFilter
//    {
//        //public void OnAuthorization(AuthorizationFilterContext context)
//        //{
//        //    var user = context.HttpContext.User;
//        //    if (!user.Identity?.IsAuthenticated ?? true)
//        //    {
//        //        context.Result = new JsonResult(ApiResponse<string>.Fail("Chưa đăng nhập"))
//        //        {
//        //            StatusCode = StatusCodes.Status401Unauthorized
//        //        };
//        //        return;
//        //    }
//        //    var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
//        //    var controllerName = actionDescriptor?.ControllerName;
//        //    var actionName = actionDescriptor?.ActionName;

//        //    if (string.IsNullOrEmpty(controllerName) || string.IsNullOrEmpty(actionName))
//        //    {
//        //        context.Result = new JsonResult(ApiResponse<string>.Fail("Không thể xác định controller/action"))
//        //        {
//        //            StatusCode = StatusCodes.Status500InternalServerError
//        //        };
//        //        return;
//        //    }

//        //    var functionCode = $"{controllerName}:{actionName}"; 

//        //    var hasPermission = user.Claims.Any(c =>
//        //        c.Type == "permission" && string.Equals(c.Value, functionCode, StringComparison.OrdinalIgnoreCase));

//        //    if (!hasPermission)
//        //    {
//        //        context.Result = new JsonResult(ApiResponse<string>.Fail("Không có quyền truy cập"))
//        //        {
//        //            StatusCode = StatusCodes.Status403Forbidden
//        //        };
//        //    }
//        //}

//        public void OnAuthorizationAsync(AuthorizationFilterContext context)
//        {
//            var user = _httpContextAccessor.HttpContext.User;
//            if (!user.Identity.IsAuthenticated)
//            {
//                context.Result = new UnauthorizedResult();
//                return;
//            }

//            // Lấy userId từ claim
//            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            if (userId == null)
//            {
//                context.Result = new UnauthorizedResult();
//                return;
//            }

//            // Xác định tên controller và action
//            var routeData = context.ActionDescriptor.RouteValues;
//            var controller = routeData["controller"];
//            var action = routeData["action"];

//            var functionKey = $"{controller.ToLower()}:{action}";

//            // Truy vấn xem user đó có quyền với functionKey không
//            var hasPermission = await _context.UserRoles
//                .Where(ur => ur.UserId == Guid.Parse(userId))
//                .SelectMany(ur => ur.Role.RolePermissions)
//                .AnyAsync(rp => rp.Function.Key == functionKey);

//            if (!hasPermission)
//            {
//                context.Result = new ForbidResult();
//            }
//        }
//    }
//}

using Infrastructure.Data;
using ManageUserSystem.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ManageUserSystem.Filter
{
    public class DynamicPermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly ILogger<DynamicPermissionFilter> _logger;

        public DynamicPermissionFilter(IHttpContextAccessor httpContextAccessor, AppDbContext context, ILogger<DynamicPermissionFilter> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _logger = logger;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                context.Result = new JsonResult(ApiResponse<string>.Fail("Người dùng chưa đăng nhập"))
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                context.Result = new JsonResult(ApiResponse<string>.Fail("Không tìm thấy thông tin người dùng trong token"))
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

            var routeData = context.ActionDescriptor.RouteValues;
            var controller = routeData["controller"]?.Trim().ToLower();
            var action = routeData["action"]?.Trim().ToLower();

            if (controller == null || action == null)
            {
                context.Result = new JsonResult(ApiResponse<string>.Fail("Không xác định được controller hoặc action"))
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return;
            }

            var functionKey = $"{controller}:{action}";
            //_logger.LogInformation($"[RBAC] Kiểm tra quyền userId={userId}, functionKey={functionKey}");

            try
            {
                var guidUserId = Guid.Parse(userId);

                var hasPermission = await _context.UserRoles
                    .Where(ur => ur.UserId == guidUserId)
                    .SelectMany(ur => ur.Role.RolePermissions)
                    .AnyAsync(rp => rp.Function.Key == functionKey);

                if (!hasPermission)
                {
                    context.Result = new JsonResult(ApiResponse<string>.Fail("Bạn không có quyền truy cập chức năng này"))
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                    return;
                }
            }
            catch (Exception ex)
            {
              //  _logger.LogError(ex, "[RBAC] Lỗi khi kiểm tra quyền truy cập");
                context.Result = new JsonResult(ApiResponse<string>.Fail("Đã xảy ra lỗi trong quá trình kiểm tra quyền truy cập"))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                return;
            }
        }
    }
}
