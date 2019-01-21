using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using App.Common.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using App.Common.Extensions;
using Newtonsoft.Json;

namespace App.Aop.Log
{
    /// <summary>
    /// 操作日志处理
    /// </summary>
    public class OperationLogHandler : LogHandler<OperateLog>
    {
        /// <summary>
        /// 构造函数初始化基本操作信息
        /// </summary>
        /// <param name="request"></param>
        public OperationLogHandler(HttpRequest request) : base(LogMode.OperateLog)
        {
            LogInfo = new OperateLog()
            {
                CreatorTime = DateTime.Now,
                ServerHost = HttpHelper.GetServerIp(),
                ClientHost = HttpHelper.GetClientIp(),
                RequestContentLength = request.ContentLength ?? 0,
                RequestType = request.Method,
                UserAgent = request.Headers[HeaderNames.UserAgent]
            };

            if (LogInfo.RequestType == "POST")
            {
                var dic = request.Form.ToDictionary(a => a.Key, b => b.Value.FirstOrDefault());
                LogInfo.RequestData = JsonConvert.SerializeObject(dic);
            }
            LogInfo.Url = request.GetAbsoluteUri();
            LogInfo.UrlReferrer = request.Headers[HeaderNames.Referer];
        }

        /// <summary>
        /// 执行时间
        /// </summary>
        public void ActionExecuted()
        {
            LogInfo.ActionExecutionTime = Math.Round((DateTime.Now - LogInfo.CreatorTime).TotalSeconds, 4);
        }

        /// <summary>
        /// 页面展示时间
        /// </summary>
        /// <param name="responseBase"></param>
        public void ResultExecuted(HttpResponse response)
        {
            LogInfo.ResponseStatus = response.StatusCode.ToString();
            //页面展示时间
            LogInfo.ResultExecutionTime = Math.Round((DateTime.Now - LogInfo.CreatorTime).TotalSeconds, 4);
        }
    }
}
