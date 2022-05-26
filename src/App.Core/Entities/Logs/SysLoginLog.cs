using System;
using SqlSugar;

namespace App.Core.Entities.Logs
{
    /// <summary>
    /// 系统用户登录日志
    /// </summary>
    [Serializable]
    public class SysLoginLog : Entity<string>
    {

        /// <summary>
        /// Ip对应地址
        /// </summary>
        public string IpAddressName { get; set; }

        /// <summary>
        /// 服务器主机名
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// 客户端主机名
        /// </summary>
        public string ClientHost { get; set; }

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string OsVersion { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 退出时间
        /// </summary>
        public DateTime? SignOutTime { get; set; }

        /// <summary>
        /// 停留时间(分钟)
        /// </summary>
        public double? StandingTime { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        public string CreateAccountId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatorTime { get; set; }
    }
}
