using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using App.Entities;
using App.IServices;
using AppSoft.Filter;
using Microsoft.AspNetCore.Mvc;

namespace AppSoft.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class ModuleController : BaseControler
    {
        private readonly ISysModuleLogic _sysModuleLogic;
        private readonly ISysPermissionLogic _sysPermissionLogic;

        public ModuleController(ISysModuleLogic sysModuleLogic, ISysPermissionLogic sysPermissionLogic)
        {
            _sysModuleLogic = sysModuleLogic;
            _sysPermissionLogic = sysPermissionLogic;
        }

        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Description("菜单列表"), AllowAccessFilter]
        public ActionResult GetMenus()
        {
            var menus = _sysModuleLogic.Queryable(m => m.DeleteMark == false, o => o.SortCode, false);
            var obj = new { code = 0, msg = "ok", data = menus, count = menus.Count() };

            return Json(obj);
        }

        /// <summary>
        /// 菜单下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet, Description("获取下拉框树形菜单"), AllowAccessFilter]
        public ActionResult GetMenuTree()
        {
            var menus = _sysModuleLogic.Queryable(m => m.DeleteMark == false && m.EnabledMark == true);
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
        [HttpGet, Description("按钮管理菜单树"), AllowAccessFilter]
        public ActionResult GetAllMenuTree()
        {
            var menus = _sysModuleLogic.Queryable(m => m.DeleteMark == false);
            var tree = GetTree(menus, "0");
            List<object> list = new List<object>() {
                    new{ id="0", name="菜单",icon="layui-icon layui-tree-branch",spread = true, children =tree}
                };
            return Json(list);
        }


        [HttpPost]
        [Description("新增/修改菜单")]
        public ActionResult Form(SysModule module)
        {
            module.CreatorAccountId = CurrentUser.AccountId;
            return Json(_sysModuleLogic.Save(module));
        }

        [HttpGet, AllowAccessFilter]
        [Description("菜单详情")]
        public ActionResult GetForm(string key)
        {
            var menu = _sysModuleLogic.FindEntity(key);
            menu.ParentId = menu.ParentId == null ? "0" : menu.ParentId;
            return Json(menu);
        }

        [HttpPost]
        [Description("启用/禁用菜单")]
        public ActionResult Enable(string id, bool status)
        {
            return Json(_sysModuleLogic.Update(m => new SysModule() { EnabledMark = status }, c => c.ModuleId == id));
        }

        [HttpPost]
        [Description("删除菜单")]
        public ActionResult Delete(string key)
        {
            return Json(_sysModuleLogic.Update(m => new SysModule() { DeleteMark = true }, c => c.ModuleId == key));
        }

        [HttpGet, Description("获取当前用户的可访问菜单按钮")]
        [AllowAccessFilter]
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
                var child = GetTree(menus, item.ModuleId);
                tree.Add(new { id = item.ModuleId, name = item.FullName, icon = item.Icon, spread = true, children = child });
            }
            return tree;
        }
    }
}