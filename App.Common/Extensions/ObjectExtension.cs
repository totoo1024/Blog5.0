using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Extensions
{
    /// <summary>
    /// 系统类型扩展
    /// </summary>
    public static class SystemExtension
    {
        /// <summary>
        /// 将对象序列化为json字符串
        /// </summary>
        /// <param name="o">需要序列化的对象</param>
        /// <param name="format">时间序列化格式</param>
        /// <returns></returns>
        public static string ToJson(this object o, string format = "yyyy-MM-dd HH:mm:ss")
        {
            if (o == null)
            {
                return "";
            }
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = format;
            return JsonConvert.SerializeObject(o, Formatting.Indented, timeFormat);
        }

        /// <summary>
        /// 将字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T Deserialize<T>(this string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 获取请求的完整URL地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }
    }
}
