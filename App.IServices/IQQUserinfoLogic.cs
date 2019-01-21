using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.Entities.Dtos;

namespace App.IServices
{
    public interface IQQUserinfoLogic : IBaseLogic<QQUserinfo>
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="code">用户验证是否被修改</param>
        /// <param name="state">自定义验证状态码</param>
        /// <returns></returns>
        QQUserinfo Login(string code, string state);

        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="url">登录成功后返回的地址</param>
        /// <returns>授权地址</returns>
        OperateResult<string> Authorize(string url);
    }
}
