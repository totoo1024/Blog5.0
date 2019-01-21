using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entities;
using App.IRepository;
using App.IServices;
using App.Common.Utils;
using App.Entities.Dtos;

namespace App.Services
{
    public class SysRoleLogic : BaseLogic<SysRole>, ISysRoleLogic
    {
        private ISysRoleRepository _sysRoleRepository;
        public SysRoleLogic(ISysRoleRepository sysRoleRepository) : base(sysRoleRepository)
        {
            _sysRoleRepository = sysRoleRepository;
        }

        /// <summary>
        /// 新增/修改角色
        /// </summary>
        /// <param name="sysRole">角色实体</param>
        /// <returns></returns>
        public OperateResult Save(SysRole sysRole)
        {
            if (QueryableCount(c => c.EnCode == sysRole.EnCode && c.RoleId != sysRole.RoleId && sysRole.DeleteMark == false) > 0)
            {
                OperateResult result = new OperateResult();
                result.Message = "角色编码已存在";
                return result;
            }
            if (string.IsNullOrWhiteSpace(sysRole.RoleId))
            {
                sysRole.RoleId = SnowflakeUtil.NextStringId();
                return Insert(sysRole);
            }
            else
            {
                return Update(sysRole, i => new { i.CreatorTime, i.CreatorAccountId, i.DeleteMark });
            }
        }
    }
}
