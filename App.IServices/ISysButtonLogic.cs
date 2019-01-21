using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entities;
using App.Entities.Dtos;

namespace App.IServices
{
    public interface ISysButtonLogic : IBaseLogic<SysButton>
    {
        /// <summary>
        /// 新增/修改按钮
        /// </summary>
        /// <param name="sysButton">按钮实体</param>
        /// <returns></returns>
        OperateResult Save(SysButton sysButton);
    }
}
