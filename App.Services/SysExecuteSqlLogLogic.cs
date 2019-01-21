using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.IRepository;
using App.IServices;
using App.Entities;

namespace App.Services
{
    class SysExecuteSqlLogLogic : BaseLogic<SysExecuteSqlLog>, ISysExecuteSqlLogLogic
    {
        ISysExecuteSqlLogRepository _sysExecuteSqlLogRepository;
        public SysExecuteSqlLogLogic(ISysExecuteSqlLogRepository sysExecuteSqlLogRepository) : base(sysExecuteSqlLogRepository)
        {
            _sysExecuteSqlLogRepository = sysExecuteSqlLogRepository;
        }
    }
}
