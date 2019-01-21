using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace App.Common.Net
{
    public static class HttpContextHelper
    {
        private static IHttpContextAccessor _accessor;
        public static HttpContext Current => _accessor.HttpContext;
        internal static void Configure(IHttpContextAccessor accessor) => _accessor = accessor;

        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void SetSession(string key, string value)
        {
            Current.Session.SetString(key, value);
        }

        /// <summary>
        /// 设置session
        /// </summary>
        /// <typeparam name="T">存储对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void SetSession<T>(string key, T value)
        {
            if (value != null)
            {
                SetSession(key, JsonConvert.SerializeObject(value));
            }
        }

        public static string GetSession(string key)
        {
            return Current.Session.GetString(key);
        }

        /// <summary>
        /// 获取session
        /// </summary>
        /// <typeparam name="T">存储对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static T GetSession<T>(string key)
        {
            string v = GetSession(key);
            if (string.IsNullOrWhiteSpace(v))
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(v);
            }
        }
    }
}
