using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using App.Application.SysManager;
using App.Application.SysManager.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace App.Hosting.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class PermissionController : AdminController
    {
        private readonly ISysPermissionService _sysPermissionService;
        private readonly ISysModuleService _sysModuleService;
        public PermissionController(ISysPermissionService sysPermissionService,
            ISysModuleService sysModuleService)
        {
            _sysPermissionService = sysPermissionService;
            _sysModuleService = sysModuleService;
        }
        [HttpGet, AllowAccess]
        [Description("获取角色授权信息")]
        public async Task<JsonResult> GetAuthorization(string key)
        {
            string AuthorizeIds = "";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var list = (await _sysPermissionService.GetListAsync(p => p.AuthorizeId == key)).Select(o => o.SysModuleId);
                AuthorizeIds = string.Join(",", list);
            }
            return Json(new { Permission = AuthorizeIds, Tree = await _sysModuleService.Tree() });
        }

        [HttpPost]
        [Description("角色分配权限")]
        public async Task<JsonResult> Index(SysPermissionInputDto dto)
        {
            dto.CreateUserId = CurrentUser.SysUserId;
            return Json(await _sysPermissionService.Save(dto));
        }
    }
}