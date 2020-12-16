using System.ComponentModel;
using System.Threading.Tasks;
using App.Application;
using App.Application.User;
using App.Core.Entities.User;
using Microsoft.AspNetCore.Mvc;

namespace App.Hosting.Areas.BlogManage.Controllers
{
    [Area("BlogManage")]
    public class QQUserController : AdminController
    {
        private readonly IQQUserService _qQUserService;

        public QQUserController(IQQUserService qQUserService)
        {
            _qQUserService = qQUserService;
        }

        [HttpPost]
        [Description("QQ用户列表")]
        public IActionResult Index(PageQueryInputDto query)
        {
            return Json(_qQUserService.GetListByPage(query), "yyyy-MM-dd HH:mm:ss");
        }

        [HttpPost]
        [Description("是否设置为博主")]
        public async Task<IActionResult> Master(string key, bool status)
        {
            return Json(await _qQUserService.UpdateAsync(qq => new QQUserinfo() { IsMaster = status }, c => c.Id == key));
        }

        [HttpPost]
        [Description("删除QQ用户")]
        public async Task<IActionResult> Delete(string key)
        {
            return Json(await _qQUserService.DeleteAsync(c => c.Id == key));
        }
    }
}