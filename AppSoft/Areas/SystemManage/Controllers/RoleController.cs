using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using App.Entities;
using App.Entities.Dtos;
using App.IServices;
using AppSoft.Filter;
using Microsoft.AspNetCore.Mvc;

namespace AppSoft.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class RoleController : BaseControler
    {
        private readonly ISysRoleLogic _sysRoleLogic;
        public RoleController(ISysRoleLogic sysRoleLogic)
        {
            _sysRoleLogic = sysRoleLogic;
        }

        [HttpPost]
        [Description("角色数据列表")]
        public ActionResult Index(QueryDto query)
        {
            return Json(_sysRoleLogic.QueryableByPage(query));
        }

        [HttpPost]
        [Description("新增/修改角色")]
        public ActionResult Form(SysRole sysRole)
        {
            sysRole.CreatorAccountId = CurrentUser.AccountId;
            return Json(_sysRoleLogic.Save(sysRole));
        }

        [HttpGet, AllowAccessFilter]
        [Description("获取角色详情")]
        public ActionResult GetForm(string key)
        {
            return Json(_sysRoleLogic.FindEntity(o => o.RoleId == key));
        }

        [HttpPost]
        [Description("启用/禁用角色")]
        public ActionResult Enable(string id, bool status)
        {
            return Json(_sysRoleLogic.Update(m => new SysRole() { EnabledMark = status }, c => c.RoleId == id));
        }

        [HttpPost]
        [Description("删除角色")]
        public JsonResult Delete(string key)
        {
            return Json(_sysRoleLogic.Update(m => new SysRole() { DeleteMark = true }, c => c.RoleId == key));
        }

        [HttpGet, AllowAccessFilter]
        [Description("获取角色下拉框")]
        public ActionResult Select()
        {
            var list = _sysRoleLogic.Queryable(m => m.EnabledMark == true && m.DeleteMark == false).Select(s => new { text = s.FullName, value = s.RoleId });
            return Json(list);
        }
    }
}