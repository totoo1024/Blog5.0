using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entities;
using App.Entities.Dtos;

namespace App.IServices
{
    public interface ISysModuleLogic : IBaseLogic<SysModule>
    {
        /// <summary>
        /// 新增/修改菜单
        /// </summary>
        /// <param name="module"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        OperateResult Save(SysModule module);

        /// <summary>
        /// 菜单按钮树
        /// </summary>
        /// <returns></returns>
        Task<List<TreeModuleDto>> Tree();
    }
}
