using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entities;
using App.IServices;
using App.IRepository;

namespace App.Services
{
    public class SysOperateLogLogic : BaseLogic<SysOperateLog>, ISysOperateLogLogic
    {
        ISysOperateLogRepository _sysOperateLogLogic;
        public SysOperateLogLogic(ISysOperateLogRepository sysOperateLogRepository) : base(sysOperateLogRepository)
        {
            _sysOperateLogLogic = sysOperateLogRepository;
        }
    }
}
