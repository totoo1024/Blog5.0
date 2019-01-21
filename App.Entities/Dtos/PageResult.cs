using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Dtos
{
    /// <summary>
    /// 分页查询统一返回类
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    public class PageResult<T>
    {
        /// <summary>
        /// 状态值
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 返回的数据对象
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string msg { get; set; } = "成功";
    }
}
