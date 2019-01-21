using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using App.IServices;
using AppSoft.Filter;
using Microsoft.AspNetCore.Mvc;

namespace AppSoft.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class PermissionController : BaseControler
    {
        private readonly ISysPermissionLogic _sysPermissionLogic;
        private readonly ISysModuleLogic _sysModuleLogic;
        public PermissionController(ISysPermissionLogic sysPermissionLogic, ISysModuleLogic sysModuleLogic)
        {
            _sysPermissionLogic = sysPermissionLogic;
            _sysModuleLogic = sysModuleLogic;
        }
        [HttpGet, AllowAccessFilter]
        [Description("获取角色授权信息")]
        public async Task<JsonResult> GetAuthorization(string key)
        {
            string AuthorizeIds = "";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var list = _sysPermissionLogic.Queryable(p => p.AuthorizeId == key).Select(o => o.SysModuleId);
                AuthorizeIds = string.Join(",", list);
            }
            return Json(new { Permission = AuthorizeIds, Tree = await _sysModuleLogic.Tree() });
        }

        [HttpPost]
        [Description("角色分配权限")]
        public async Task<JsonResult> Index(string roleId, List<string> permission)
        {
            return Json(await _sysPermissionLogic.Save(roleId, permission, CurrentUser.AccountId));
        }
    }
}