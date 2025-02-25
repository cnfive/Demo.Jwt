﻿using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication5.AuthManagement
{
    public class PolicyHandler : AuthorizationHandler<PolicyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            //赋值用户权限
            var userPermissions = requirement.UserPermissions;
            //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息
          //  var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext).HttpContext;
            //请求Url
     //       var questUrl = httpContext.Request.Path.Value.ToUpperInvariant();
            //是否经过验证
       //     var isAuthenticated = httpContext.User.Identity.IsAuthenticated;

            var questUrl = (context.Resource as Microsoft.AspNetCore.Routing.RouteEndpoint).RoutePattern.RawText.ToString();

            //是否经过验证

            var isAuthenticated = context.User.Identity.IsAuthenticated;

          //  var isAuthenticated = context.User.Identity.IsAuthenticated;


            if (isAuthenticated)
            {
                if (userPermissions.GroupBy(g => g.Url).Any(w => w.Key.ToUpperInvariant() == questUrl))
                {
                    //用户名
                //    var userName = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value;

                    var userName = context.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value;
                    //     if (userPermissions.Any(w => w.UserName == userName && w.Url.ToUpperInvariant() == questUrl))

                    //     if (userPermissions.Any(w => w.RoleName == userName && w.Url == questUrl.ToString()))
                    if (userPermissions.Any(w => w.UserName == userName && w.Url == questUrl.ToString()))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        //无权限跳转到拒绝页面
                        //  httpContext.Response.Redirect(requirement.DeniedAction);
                        context.Fail();
                      //  context.respon
                    }
                }
                else
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}


