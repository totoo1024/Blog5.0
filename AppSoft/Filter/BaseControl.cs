using App.Common.Log;
using App.Common.Auth;
using AppSoft.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppSoft
{
    [Authorization, ExceptionGlobalFilter]
    public class BaseControler : Controller, IResultFilter
    {
        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        public virtual AuthorizationUser CurrentUser { get => AuthenticationHelper.Current(); }

        /// <summary>
        /// 操作日志处理
        /// </summary>
        private OperationLogHandler _operationLogHandler;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _operationLogHandler = new OperationLogHandler(context.HttpContext.Request);
            _operationLogHandler.LogInfo.CreateAccountId = CurrentUser?.AccountId;
            _operationLogHandler.LogInfo.CreateUserName = CurrentUser == null ? "匿名用户" : CurrentUser.RealName;
            _operationLogHandler.LogInfo.ControllerName = context.ActionDescriptor.RouteValues["controller"];
            _operationLogHandler.LogInfo.ActionName = context.ActionDescriptor.RouteValues["action"];
            var des = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(false).Where(f => f is DescriptionAttribute).FirstOrDefault();
            if (des != null)
            {
                _operationLogHandler.LogInfo.Description = (des as DescriptionAttribute).Description;
            }
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            _operationLogHandler.ActionExecuted();
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result.GetType() == typeof(RedirectResult))
            {
                _operationLogHandler.ActionExecuted();
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            _operationLogHandler.ResultExecuted(context.HttpContext.Response);
            _operationLogHandler.WriteLog();
        }

        /// <summary>
        /// 创建 JsonResult 对象，该对象使用指定 JSON 请求行为将指定对象序列化为 JavaScript 对象表示法 (JSON) 格式。
        /// </summary>
        /// <param name="data">要序列化的 JavaScript 对象图</param>
        /// <param name="format">时间格式</param>
        /// <returns></returns>
        public JsonResult Json(object data, string format)
        {
            var setting = new JsonSerializerSettings();
            setting.DateFormatString = format;
            return base.Json(data, setting);
        }

        #region 基本视图
        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public virtual IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 新增/编辑页
        /// </summary>
        /// <returns></returns>
        public virtual IActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 详情页
        /// </summary>
        /// <returns></returns>
        public virtual IActionResult Details()
        {
            return View();
        }
        #endregion
    }
}
