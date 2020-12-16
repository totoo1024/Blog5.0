using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using App.Application.SysManager;
using App.Application.SysManager.Dtos;
using App.Core.Entities.SysManager;
using Microsoft.AspNetCore.Mvc;

namespace App.Hosting.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class ModuleController : AdminController
    {
        private readonly ISysModuleService _sysModuleService;
        private readonly ISysPermissionService _sysPermissionLogic;

        public ModuleController(ISysModuleService sysModuleService,
            ISysPermissionService sysPermissionService)
        {
            _sysModuleService = sysModuleService;
            _sysPermissionLogic = sysPermissionService;
        }

        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Description("菜单列表"), AllowAccess]
        public async Task<ActionResult> GetMenus()
        {
            var menus = await _sysModuleService.GetListCacheAsync(m => m.DeleteMark == false, o => o.SortCode, false);
            var obj = new { code = 0, msg = "ok", data = menus, count = menus.Count() };

            return Json(obj);
        }

        /// <summary>
        /// 菜单下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet, Description("获取下拉框树形菜单"), AllowAccess]
        public async Task<ActionResult> GetMenuTree()
        {
            var menus = await _sysModuleService.GetListCacheAsync(m => m.DeleteMark == false && m.EnabledMark == true);
            var tree = GetTree(menus, "0");
            List<object> list = new List<object>() {
                    new{ id="0", name="父节点",icon="seraph icon-viewheadline",spread = true, children =tree}
                };
            return Json(list);
        }

        /// <summary>
        /// 按钮管理菜单树
        /// </summary>
        /// <returns></returns>
        [HttpGet, Description("按钮管理菜单树"), AllowAccess]
        public async Task<ActionResult> GetAllMenuTree()
        {
            var menus = await _sysModuleService.GetListCacheAsync(m => m.DeleteMark == false);
            var tree = GetTree(menus, "0");
            List<object> list = new List<object>() {
                    new{ id="0", name="菜单",icon="layui-icon layui-tree-branch",spread = true, children =tree}
                };
            return Json(list);
        }


        [HttpPost]
        [Description("新增/修改菜单")]
        public async Task<ActionResult> Form(SysModuleInputDto module)
        {
            module.CreatorAccountId = CurrentUser.AccountId;
            return Json(await _sysModuleService.Save(module));
        }

        [HttpGet, AllowAccess]
        [Description("菜单详情")]
        public async Task<ActionResult> GetForm(string key)
        {
            var menu = await _sysModuleService.FindEntityAsync(key);
            menu.ParentId ??= "0";
            return Json(menu);
        }

        [HttpPost]
        [Description("启用/禁用菜单")]
        public async Task<ActionResult> Enable(string id, bool status)
        {
            return Json(await _sysModuleService.UpdateRemoveCacheAsync(m => new SysModule() { EnabledMark = status }, c => c.Id == id));
        }

        [HttpPost]
        [Description("删除菜单")]
        public async Task<ActionResult> Delete(string key)
        {
            return Json(await _sysModuleService.UpdateRemoveCacheAsync(m => new SysModule() { DeleteMark = true }, c => c.Id == key));
        }

        [HttpGet, Description("获取当前用户的可访问菜单按钮")]
        [AllowAccess]
        public async Task<ActionResult> GetMenuAndButton()
        {
            return Json(await _sysPermissionLogic.GetMenuAndButton(CurrentUser.RoleId));
        }

        /// <summary>
        /// 获取菜单树（递归）
        /// </summary>
        /// <param name="menus">所有菜单</param>
        /// <param name="pid">父级id</param>
        /// <returns></returns>
        private List<object> GetTree(List<SysModule> menus, string pid)
        {
            List<object> tree = new List<object>();
            var list = menus.Where(m => m.ParentId == pid).OrderBy(o => o.SortCode);
            foreach (var item in list)
            {
                var child = GetTree(menus, item.Id);
                tree.Add(new { id = item.Id, name = item.FullName, icon = item.Icon, spread = true, children = child });
            }
            return tree;
        }
    }
}