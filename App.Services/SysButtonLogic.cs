using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Common.Utils;
using App.Entities;
using App.Entities.Dtos;
using App.IRepository;
using App.IServices;

namespace App.Services
{
    public class SysButtonLogic : BaseLogic<SysButton>, ISysButtonLogic
    {
        ISysButtonRepository _sysButtonRepository;
        public SysButtonLogic(ISysButtonRepository sysButtonRepository) : base(sysButtonRepository)
        {
            _sysButtonRepository = sysButtonRepository;
        }

        /// <summary>
        /// 新增/修改按钮
        /// </summary>
        /// <param name="sysButton">按钮实体</param>
        /// <returns></returns>
        public OperateResult Save(SysButton sysButton)
        {
            if (_sysButtonRepository.QueryableCount(c => c.EnCode == sysButton.EnCode && c.ButtonId != sysButton.ButtonId && c.SysModuleId == sysButton.SysModuleId) > 0)
            {
                OperateResult result = new OperateResult();
                result.Message = "按钮编码已存在";
                return result;
            }
            if (string.IsNullOrEmpty(sysButton.ButtonId))
            {
                sysButton.ButtonId = SnowflakeUtil.NextStringId();
                return Insert(sysButton);
            }
            else
            {
                return Update(sysButton, i => new { i.SysModuleId, i.CreatorTime, i.CreatorAccountId });
            }
        }
    }
}
