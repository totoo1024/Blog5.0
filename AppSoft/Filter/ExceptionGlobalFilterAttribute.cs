using App.Aop.Log;
using App.Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace AppSoft.Filter
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExceptionGlobalFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            //写入日志到数据库
            ExceptionLogHandler exceptionLog = new ExceptionLogHandler(context.Exception);
            exceptionLog.WriteLog();
            if (context.HttpContext.Request.Method.ToLower() == "post" || context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
            {
                OperateResult result = new OperateResult("系统异常，请联系管理员处理", ResultStatus.Error);
                context.Result = new JsonResult(result);
            }
            else
            {
                //跳转至错误页
                context.HttpContext.Response.WriteAsync("<script type=\"text/javascript\">top.location.href = '/Main/Login/Index'</script>");
            }
        }
    }
}
