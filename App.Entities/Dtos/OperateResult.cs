using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Dtos
{
    /// <summary>
    /// 业务层增删改返回该类型
    /// </summary>
    public class OperateResult
    {
        public OperateResult()
        {
            Status = ResultStatus.Error;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="msg">提示消息</param>
        /// <param name="status">操作状态</param>
        public OperateResult(string msg, ResultStatus status = ResultStatus.Error)
        {
            Status = status;
            Message = msg;
        }

        private ResultStatus _status;
        /// <summary>
        /// 状态值
        /// </summary>
        public ResultStatus Status
        {
            get { return _status; }
            set
            {
                Message = value == ResultStatus.Success ? "操作成功" : "操作失败";
                _status = value;
            }
        }

        /// <summary>
        /// 消息提示
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// 业务层增删改返回该类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OperateResult<T> : OperateResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// 操作状态值
    /// </summary>
    public enum ResultStatus
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 警告
        /// </summary>
        Warning = 1,

        /// <summary>
        /// 操作引发错误
        /// </summary>
        Error = 2,

        /// <summary>
        /// 登录过期
        /// </summary>
        SignOut = 401,

        /// <summary>
        /// 无权访问
        /// </summary>
        NoAccess = 403
    }
}
