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
    public class ButtonController : BaseControler
    {
        private readonly ISysButtonLogic _sysButtonLogic;
        public ButtonController(ISysButtonLogic sysButtonLogic)
        {
            _sysButtonLogic = sysButtonLogic;
        }
        [HttpPost]
        [Description("获取菜单下所有按钮")]
        public IActionResult Index(string moduleId)
        {
            var list = _sysButtonLogic.Queryable(b => b.SysModuleId == moduleId, o => o.SortCode, false);
            var obj = new { code = 0, msg = "成功", count = list.Count, data = list };
            return Json(obj);
        }

        [HttpPost]
        [Description("新增按钮")]
        public IActionResult Form(SysButton button)
        {
            button.EnCode = button.JsEvent;
            button.CreatorAccountId = CurrentUser.AccountId;
            return Json(_sysButtonLogic.Save(button));
        }

        [HttpGet, Description("获取按钮详情")]
        [AllowAccessFilter]
        public IActionResult GetForm(string key)
        {
            return Json(_sysButtonLogic.FindEntity(b => b.ButtonId == key));
        }

        [HttpPost]
        [Description("删除按钮")]
        public IActionResult Delete(string key)
        {
            return Json(_sysButtonLogic.Delete(key));
        }
    }
}