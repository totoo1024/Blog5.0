using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.IServices;
using App.Entities;
using App.IRepository;

namespace App.Services
{
    public class SysLoginLogLogic : BaseLogic<SysLoginLog>, ISysLoginLogLogic
    {
        ISysLoginLogRepository _sysLoginLogRepository;
        public SysLoginLogLogic(ISysLoginLogRepository sysLoginLogRepository) : base(sysLoginLogRepository)
        {
            _sysLoginLogRepository = sysLoginLogRepository;
        }
    }
}
