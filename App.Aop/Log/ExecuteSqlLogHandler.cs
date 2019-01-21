using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Diagnostics;
using App.Common.Auth;
using App.Common.Net;
using App.Common.Utils;

namespace App.Aop.Log
{
    public class ExecuteSqlLogHandler : LogHandler<ExecuteSqlLog>
    {
        Stopwatch stopwatch = new Stopwatch();
        public ExecuteSqlLogHandler(string sql, string parm) : base(LogMode.SqlLog)
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
            LogInfo = new ExecuteSqlLog()
            {
                SqlCommand = sql,
                Parameter = parm,
                CreateAccountId = currentUser.AccountId,
                CreateUserName = currentUser.RealName,
                CreatorTime = DateTime.Now
            };
            stopwatch.Start();
        }

        public override void WriteLog()
        {
            stopwatch.Stop();
            LogInfo.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
            base.WriteLog();
        }
    }
}
