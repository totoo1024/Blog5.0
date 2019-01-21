using App.IRepository;
using App.IServices;
using App.Entities;
using App.Entities.Dtos;
using App.Common.Extensions;
using App.Common.Utils;
using App.Common.Auth;
using System.Collections.Generic;

namespace App.Services
{
    public class SysAccountLogic : BaseLogic<SysAccount>, ISysAccountLogic
    {
        ISysAccountRepository _sysAccountRepository;
        ISysUserLogic _sysUserLogic;
        public SysAccountLogic(ISysAccountRepository sysAccountRepository, ISysUserLogic sysUserLogic) : base(sysAccountRepository)
        {
            _sysAccountRepository = sysAccountRepository;
            _sysUserLogic = sysUserLogic;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public OperateResult<AuthorizationUser> Login(string account, string password)
        {
            var accountDetail = _sysAccountRepository.AccountDetail(a => a.UserName == account);
            OperateResult<AuthorizationUser> result = new OperateResult<AuthorizationUser>();
            if (accountDetail == null)
            {
                result.Message = "用户名或密码错误";
                return result;
            }
            if (accountDetail.Password != EncryptUtil.MD5Encrypt32(accountDetail.AccountId + password))
            {
                result.Message = "用户名或密码错误";

            }
            else if (!accountDetail.EnabledMark)
            {
                result.Message = "登录账户已被禁用";
            }
            else
            {
                result.Status = ResultStatus.Success;
                var user = accountDetail.MapTo<AuthorizationUser, AccountDetailsDto>();
                user.LoginId = SnowflakeUtil.NextStringId();
                user.HeadIcon = user.HeadIcon ?? "/images/default.png";
                result.Data = user;
            }
            return result;
        }

        /// <summary>
        /// 分页查询用户详细信息
        /// </summary>
        /// <param name="queryDto">分页查询实体</param>
        /// <returns></returns>
        public PageResult<List<AccountDetailsDto>> AccountPagging(QueryDto query)
        {
            return _sysAccountRepository.AccountPaging(query);

        }

        /// <summary>
        /// 获取系统账户详细信息
        /// </summary>
        /// <param name="accountId">账户id</param>
        /// <returns></returns>
        public AccountDetailsDto AccountDetail(string accountId)
        {
            return _sysAccountRepository.AccountDetail(accountId);
        }

        /// <summary>
        /// 新增/编辑系统用户
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        public OperateResult Save(AccountDetailsDto dto, string createUserId)
        {
            var account = dto.MapTo<SysAccount, AccountDetailsDto>();
            var user = dto.MapTo<SysUser, AccountDetailsDto>();
            if (_sysAccountRepository.QueryableCount(a => a.UserName == account.UserName && a.AccountId != dto.AccountId) > 0)
            {
                return new OperateResult() { Message = "用户名已存在" };
            }
            //添加
            if (string.IsNullOrWhiteSpace(dto.AccountId))
            {
                account.AccountId = SnowflakeUtil.NextStringId();
                account.CreatorAccountId = createUserId;
                user.AccountId = account.AccountId;
                user.UserId = SnowflakeUtil.NextStringId();
                account.Password = EncryptUtil.MD5Encrypt32(account.AccountId + account.Password);
                var result = Insert(account);
                if (result.Status == ResultStatus.Success)
                {
                    result = _sysUserLogic.Insert(user);
                }
                return result;
            }
            else
            {
                //编辑
                var result = Update(a => new SysAccount() { UserName = account.UserName, RoleId = account.RoleId }, c => c.AccountId == dto.AccountId);
                if (result.Status == ResultStatus.Success)
                {
                    result = _sysUserLogic.Update(user, c => c.AccountId == dto.AccountId, ig => new { ig.UserId, ig.AccountId });
                }
                return result;
            }
        }

        /// <summary>
        /// 重置系统用户密码
        /// </summary>
        /// <param name="accountId">账户id</param>
        /// <param name="password">新密码</param>
        /// <param name="rePassword">确认密码</param>
        /// <returns></returns>
        public OperateResult ResetPassword(string accountId, string password, string rePassword)
        {
            if (password != rePassword)
            {
                return new OperateResult() { Message = "两次输入的密码不一致" };
            }
            password = EncryptUtil.MD5Encrypt32(accountId + password);
            return Update(a => new SysAccount() { Password = password }, c => c.AccountId == accountId);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="accountId">用户ID</param>
        /// <param name="oldPwd">原始密码</param>
        /// <param name="password">新密码</param>
        /// <returns></returns>
        public OperateResult ChangePwd(string accountId, string oldPwd, string password)
        {
            SysAccount account = FindEntity(c => c.AccountId == accountId);
            if (account.Password != EncryptUtil.MD5Encrypt32(accountId + oldPwd))
            {
                return new OperateResult() { Message = "输入的原始密码错误" };
            }
            if (password.Length < 6)
            {
                return new OperateResult() { Message = "密码长度不能少于6个字符" };
            }
            password = EncryptUtil.MD5Encrypt32(accountId + password);
            return Update(a => new SysAccount() { Password = password }, c => c.AccountId == accountId);
        }
    }
}
