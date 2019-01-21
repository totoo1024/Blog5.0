using App.Common.Extensions;
using App.Common.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Auth
{
    /// <summary>
    /// 用户登录保存用户状态
    /// </summary>
    public class AuthenticationHelper
    {
        /// <summary>
        /// 写入cookie
        /// </summary>
        /// <param name="authorizationUser">用户登录信息</param>
        public static Task SetAuthCookie(AuthorizationUser authorizationUser)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(authorizationUser)));
            return HttpContextHelper.Current.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        public static AuthorizationUser Current()
        {
            var auth = HttpContextHelper.Current.User;
            if (auth.Identity.IsAuthenticated)
            {
                string info = auth.Claims.FirstOrDefault()?.Value;
                return info.Deserialize<AuthorizationUser>();
            }
            return null;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public static Task SignOut()
        {
            return HttpContextHelper.Current.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
