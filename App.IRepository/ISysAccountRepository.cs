using App.Core;
using App.Entities;
using App.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace App.IRepository
{
    public interface ISysAccountRepository : IBaseRepository<SysAccount>
    {
        /// <summary>
        /// 分页查询用户详细信息
        /// </summary>
        /// <param name="queryDto">分页查询实体</param>
        /// <returns></returns>
        PageResult<List<AccountDetailsDto>> AccountPaging(QueryDto queryDto);

        /// <summary>
        /// 获取系统账户详细信息
        /// </summary>
        /// <param name="accountId">账户id</param>
        /// <returns></returns>
        AccountDetailsDto AccountDetail(string accountId);

        /// <summary>
        /// 获取系统账户详细信息
        /// </summary>
        /// <param name="expression">查询条件</param>
        /// <returns></returns>
        AccountDetailsDto AccountDetail(Expression<Func<SysAccount, bool>> expression);
    }
}
