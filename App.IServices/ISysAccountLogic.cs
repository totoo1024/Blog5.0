using System;
using System.Collections.Generic;
using App.Common.Auth;
using App.Entities;
using App.Entities.Dtos;

namespace App.IServices
{
    public interface ISysAccountLogic : IBaseLogic<SysAccount>
    {
        /// <summary>
        /// 分页查询用户详细信息
        /// </summary>
        /// <param name="queryDto">分页查询实体</param>
        /// <returns></returns>
        PageResult<List<AccountDetailsDto>> AccountPagging(QueryDto queryDto);

        /// <summary>
        /// 获取系统账户详细信息
        /// </summary>
        /// <param name="accountId">账户id</param>
        /// <returns></returns>
        AccountDetailsDto AccountDetail(string accountId);

        /// <summary>
        /// 新增/编辑系统用户
        /// </summary>
        /// <param name="dto">账户基本信息</param>
        /// <param name="createUserId">创建人账户id</param>
        /// <returns></returns>
        OperateResult Save(AccountDetailsDto dto, string createAccountId);

        /// <summary>
        /// 重置系统用户密码
        /// </summary>
        /// <param name="accountId">账户id</param>
        /// <param name="password">新密码</param>
        /// <param name="rePassword">确认密码</param>
        /// <returns></returns>
        OperateResult ResetPassword(string accountId, string password, string rePassword);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        OperateResult<AuthorizationUser> Login(string account, string password);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="accountId">用户ID</param>
        /// <param name="oldPwd">原始密码</param>
        /// <param name="password">新密码</param>
        /// <returns></returns>
        OperateResult ChangePwd(string accountId, string oldPwd, string password);
    }
}
