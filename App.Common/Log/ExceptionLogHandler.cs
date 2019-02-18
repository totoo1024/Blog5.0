using App.Common.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using App.Common.Net;
using App.Common.Extensions;

namespace App.Common.Log
{
    /// <summary>
    /// 异常日志处理
    /// </summary>
    public class ExceptionLogHandler : LogHandler<ExceptionLog>
    {
        public ExceptionLogHandler(Exception exception) : base(LogMode.ExceptionLog)
        {
            AuthorizationUser currentUser = null;
            var current = HttpContextHelper.Current;
            if (current != null)
            {
                currentUser = AuthenticationHelper.Current();
            }
            if (currentUser == null)
            {
                currentUser = new AuthorizationUser()
                {
                    RealName = "匿名用户"
                };
            }
            LogInfo = new ExceptionLog
            {
                CreatorTime = DateTime.Now,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                ExceptionType = exception.GetType().FullName,
                CreateUserName = currentUser.RealName,
                CreateaAcountId = currentUser.AccountId,
                ServerHost = HttpHelper.GetServerIp(),
                ClientHost = HttpHelper.GetClientIp(),
                Runtime = "Web"
            };
            //获取请求信息
            var request = HttpContextHelper.Current.Request;
            LogInfo.RequestUrl = request.GetAbsoluteUri();
            LogInfo.HttpMethod = request.Method;
            LogInfo.UserAgent = HttpHelper.UserAgent();
            var inputStream = request.Body;
            var streamReader = new StreamReader(inputStream);
            var requestData = HttpUtility.UrlDecode(streamReader.ReadToEnd());
            //读取完数据流后重置当前流的位置（很重要）
            //request.Body.Position = 0;
            LogInfo.RequestData = requestData;
            LogInfo.InnerException = NLogger.GetExceptionFullMessage(exception.InnerException);
        }
    }
}
