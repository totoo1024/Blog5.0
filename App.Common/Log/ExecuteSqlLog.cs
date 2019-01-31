using System;
using System.Runtime.Serialization;
using App.Common.Utils;
using Newtonsoft.Json;

namespace App.Common.Log
{
    /// <summary>
    /// 执行sql记录实体
    /// </summary>
    public class ExecuteSqlLog
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string ExecuteId { get; set; } = SnowflakeUtil.NextStringId();

        /// <summary>
        /// SQL命令
        /// </summary>
        public string SqlCommand { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// 耗时(单位：秒)
        /// </summary>
        public double ElapsedTime { get; set; }

        /// <summary>
        /// 是否执行失败(0：成功；1：失败)
        /// </summary>
        public int IsFail { get; set; }

        /// <summary>
        /// 执行SQL错误消息
        /// </summary>
        public string Massage { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        public string CreateAccountId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonConverter(typeof(DateTimeFormat))]
        public DateTime CreatorTime { get; set; }
    }
}
