using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entities;
using App.IRepository;
using App.IServices;

namespace App.Services
{
    public class SysUserLogic : BaseLogic<SysUser>, ISysUserLogic
    {
        private ISysUserRepository _sysUserRepository;
        public SysUserLogic(ISysUserRepository sysUserRepository) : base(sysUserRepository)
        {
            _sysUserRepository = sysUserRepository;
        }
    }
}
