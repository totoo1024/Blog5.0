using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using App.Entities.Dtos;
using App.Common.Extensions;
using App.Common.Auth;
using AppSoft.Autofac;
using App.IServices;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace AppSoft.Filter
{
    /// <summary>
    /// 检验用户登录和权限验证过滤器（过滤器只能用于类和方法）有AllowAnonymousAttribute标记忽略验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public bool ig { get; set; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var auth = context.HttpContext.User;
            string info = auth.Claims.FirstOrDefault()?.Value;
            if (!auth.Identity.IsAuthenticated && !context.Filters.Any(filter => filter is IAllowAnonymousFilter))
            {
                if (context.HttpContext.Request.Method.ToLower() == "post" || context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
                {
                    OperateResult result = new OperateResult("登录已过期", ResultStatus.SignOut);
                    context.Result = new JsonResult(result);
                }
                else
                {
                    context.HttpContext.Response.WriteAsync("<script type=\"text/javascript\">top.location.href = '/Main/Login/Index'</script>");
                }
            }
            else
            {
                //是否忽略权限验证
                bool ignore = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(false).Any(f => f is AllowAccessFilterAttribute);
                if (!ignore)
                {
                    var currentUser = info.Deserialize<AuthorizationUser>();
                    var _sysPermissionLogic = EngineContext.Current.Resolve<ISysPermissionLogic>();
                    var dic = context.ActionDescriptor.RouteValues;
                    //区域
                    string area = dic["area"];
                    //控制器
                    string controller = dic["controller"];
                    //action名
                    string action = dic["action"];
                    if (area != null)
                    {
                        area = $"/{area}";
                    }
                    //当前请求地址的虚拟路径
                    string currUrl = $"{area}/{controller}/{action}";
                    bool isAuthorize = _sysPermissionLogic.CheckPermission(currentUser.RoleId, currUrl);
                    if (!isAuthorize)
                    {
                        //没有权限
                        if (context.HttpContext.Request.Method.ToLower() == "post" || context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
                        {
                            OperateResult result = new OperateResult("无权访问", ResultStatus.NoAccess);
                            context.Result = new JsonResult(result);
                        }
                        else
                        {
                            //context.Result = new RedirectToActionResult("Index", "Login", new { area = "Main" });
                            //跳转到相关页面
                            context.HttpContext.Response.WriteAsync("<script type=\"text/javascript\">top.location.href = '/Main/Login/Index'</script>");
                        }
                    }
                }
            }
        }
    }
}
