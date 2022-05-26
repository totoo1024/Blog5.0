using System.ComponentModel;
using System.Threading.Tasks;
using App.Application;
using App.Application.Blog;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using Microsoft.AspNetCore.Mvc;

namespace App.Hosting.Areas.BlogManage.Controllers
{
    [Area("BlogManage")]
    public class NoticeController : AdminController
    {
        private readonly INoticeService _noticeService;

        public NoticeController(INoticeService noticeService)
        {
            _noticeService = noticeService;
        }

        [HttpPost]
        [Description("通知列表")]
        public IActionResult Index(PageQueryInputDto query)
        {
            return Json(_noticeService.GetListByPage(query), "yyyy-MM-dd HH:mm:ss");
        }

        [HttpPost]
        [Description("新增/编辑通知")]
        public async Task<IActionResult> Form(NoticeInputDto notice)
        {
            return Json(await _noticeService.Save(notice));
        }

        [AllowAccess]
        [Description("通知详情")]
        public async Task<IActionResult> Detail(string key)
        {
            return Json(await _noticeService.FindEntityAsync(c => c.Id == key));
        }

        [HttpPost]
        [Description("删除通知")]
        public async Task<IActionResult> Delete(string key)
        {
            return Json(await _noticeService.UpdateRemoveCacheAsync(n => new Noticeinfo() { DeleteMark = true }, c => c.Id == key));
        }
    }
}