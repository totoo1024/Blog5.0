using System;
using SqlSugar;

namespace App.Core.Entities.User
{
    /// <summary>
    /// 记录QQ授权后信息接入QQ授权登录
    /// </summary>
    public class QQUserinfo : Entity<string>
    {
        /// <summary>
        /// QQ授权ID
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NikeName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// QQ头像40*40
        /// </summary>
        public string Image40 { get; set; }

        /// <summary>
        /// QQ头像100*100
        /// </summary>
        public string Image100 { get; set; }

        /// <summary>
        /// 是否是博主
        /// </summary>
        public bool IsMaster { get; set; } = false;

        /// <summary>
        /// 最后登录日期
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// QQ授权token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// token过期时间
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// 换取token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatorTime { get; set; } = DateTime.Now;
    }
}
