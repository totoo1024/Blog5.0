using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entities;
using App.Entities.Dtos;

namespace App.IServices
{
    public interface ISysRoleLogic : IBaseLogic<SysRole>
    {
        /// <summary>
        /// 新增/修改角色
        /// </summary>
        /// <param name="sysRole"></param>
        /// <returns></returns>
        OperateResult Save(SysRole sysRole);
    }
}
