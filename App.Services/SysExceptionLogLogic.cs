using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.IRepository;
using App.Entities;
using App.IServices;

namespace App.Services
{
    public class SysExceptionLogLogic : BaseLogic<SysExceptionLog>, ISysExceptionLogLogic
    {
        ISysExceptionLogRepository _sysExceptionLogRepository;
        public SysExceptionLogLogic(ISysExceptionLogRepository sysExceptionLogRepository) : base(sysExceptionLogRepository)
        {
            _sysExceptionLogRepository = sysExceptionLogRepository;
        }
    }
}
