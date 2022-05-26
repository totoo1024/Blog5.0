using System.ComponentModel;
using System.Threading.Tasks;
using App.Application.SysManager;
using App.Application.SysManager.Dtos;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace App.Hosting.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class ButtonController : AdminController
    {
        private readonly ISysButtonService _sysButtonService;
        private readonly SqlSugarClient _sqlSugarClient;

        public ButtonController(ISysButtonService sysButtonLogic)
        {
            _sysButtonService = sysButtonLogic;
        }
        [HttpPost]
        [Description("获取菜单下所有按钮")]
        public async Task<ActionResult> Index(string moduleId)
        {
            var list = await _sysButtonService.GetListAsync(b => b.SysModuleId == moduleId, o => o.SortCode, false);
            var obj = new { code = 0, msg = "成功", count = list.Count, data = list };
            return Json(obj);
        }

        [HttpPost]
        [Description("新增按钮")]
        public async Task<ActionResult> Form(SysButtonInputDto button)
        {
            button.EnCode = button.JsEvent;
            button.CreatorAccountId = CurrentUser.AccountId;
            return Json(await _sysButtonService.Save(button));
        }

        [HttpGet, Description("获取按钮详情")]
        [AllowAccess]
        public async Task<ActionResult> GetForm(string key)
        {
            return Json(await _sysButtonService.FindEntityAsync(b => b.Id == key));
        }

        [HttpPost]
        [Description("删除按钮")]
        public async Task<ActionResult> Delete(string key)
        {
            return Json(await _sysButtonService.DeleteRemoveCacheAsync(key));
        }
    }
}