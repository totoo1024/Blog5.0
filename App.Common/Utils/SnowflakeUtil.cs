using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Utils
{
    /// <summary>
    /// 雪花ID（全局唯一）
    /// </summary>
    public class SnowflakeUtil
    {
        private static readonly IdWorker idWorker = new IdWorker(1, 1);
        private static readonly object lockObj = new object();

        /// <summary>
        /// 获取long类型唯一ID
        /// </summary>
        /// <returns></returns>
        public static long NextId()
        {
            lock (lockObj)
            {
                return idWorker.NextId();
            }
        }

        /// <summary>
        /// 获取字符串类型唯一ID
        /// </summary>
        /// <returns></returns>
        public static string NextStringId()
        {
            return NextId().ToString();
        }
    }
}
