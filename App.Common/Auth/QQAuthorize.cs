using System;
using System.Collections.Generic;
using System.Text;
using App.Common.Utils;
using App.Common.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace App.Common.Auth
{
    public class QQAuthorize
    {
        private const string AppId = "您的AppId";

        private const string AppKey = "您的AppKey";

        private const string CallbackUrl = "http://www.miboxapp.com/home/login";

        /// <summary>
        /// 获取授权登录地址
        /// </summary>
        /// <param name="cururl">授权成功后返回的地址</param>
        /// <returns></returns>
        public static string AuthorizeLoginUrl(string url)
        {
            string state = SnowflakeUtil.NextStringId();
            HttpContextHelper.Current.Session.SetString("lib" + state, url);
            return $"https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id={AppId}&redirect_uri={CallbackUrl}&state={state}";
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="code">回调code</param>
        /// <param name="state">用户验证是否合法请求</param>
        /// <returns></returns>
        public static AccessTokenInfo GetAccessToken(string code, string state)
        {
            if (string.IsNullOrWhiteSpace(HttpContextHelper.Current.Session.GetString("lib" + state)))
            {
                throw new Exception("缺少参数");
            }
            string url = $"https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id={AppId}&client_secret={AppKey}&code={code}&redirect_uri={System.Net.WebUtility.UrlEncode(CallbackUrl)}";

            string result = HttpHelper.HttpGet(url);
            AccessTokenInfo token;
            if (result.Contains("callback"))
            {
                int s = result.IndexOf("(");
                int e = result.IndexOf(")");
                result = result.Substring(s + 1, e - s - 1);
                token = JsonConvert.DeserializeObject<AccessTokenInfo>(result);
            }
            else
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string[] arr = result.Split('&');
                foreach (string item in arr)
                {
                    string[] temp = item.Split('=');
                    dic[temp[0]] = temp[1];
                }
                token = new AccessTokenInfo()
                {
                    access_token = dic["access_token"],
                    expires_in = Convert.ToInt32(dic["expires_in"]),
                    refresh_token = dic["refresh_token"]
                };
            }
            if (!string.IsNullOrWhiteSpace(token.error))
            {
                throw new Exception(token.error_description);
            }
            return token;
        }

        /// <summary>
        /// 获取授权用户的openid
        /// </summary>
        /// <param name="token">accesstoken令牌</param>
        /// <returns></returns>
        public static string GetOpenId(string accesstoken)
        {
            string url = $"https://graph.qq.com/oauth2.0/me?access_token={accesstoken}";
            string result = HttpHelper.HttpGet(url);
            if (result.Contains("callback"))
            {
                int s = result.IndexOf("(");
                int e = result.IndexOf(")");
                result = result.Substring(s + 1, e - s - 1);
            }
            return JsonConvert.DeserializeObject<OpenIdInfo>(result)?.openid;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accesstoken">accesstoken令牌</param>
        /// <param name="openid">用户的openid</param>
        /// <returns></returns>
        public static UserInfo GetUserInfo(string accesstoken, string openid)
        {
            string url = $"https://graph.qq.com/user/get_user_info?access_token={accesstoken}&oauth_consumer_key={AppId}&openid={openid}";
            string result = HttpHelper.HttpGet(url);
            return JsonConvert.DeserializeObject<UserInfo>(result);
        }
    }
}
