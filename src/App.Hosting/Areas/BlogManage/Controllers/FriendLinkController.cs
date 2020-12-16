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
    public class FriendLinkController : AdminController
    {
        private readonly IFriendLinkService _friendLinkService;

        public FriendLinkController(IFriendLinkService friendLinkService)
        {
            _friendLinkService = friendLinkService;
        }
        [HttpPost]
        [Description("友情链接信息列表")]
        public IActionResult Index(PageQueryInputDto query)
        {
            return Json(_friendLinkService.GetListByPage(query), "yyyy-MM-dd HH:mm:ss");
        }

        [Description("新增/编辑友情链接信息")]
        [HttpPost]
        public async Task<IActionResult> Form(FriendLinkInputDto friendLink)
        {
            return Json(await _friendLinkService.Save(friendLink));
        }

        [Description("友情链接详情")]
        [AllowAccess]
        public async Task<IActionResult> Detail(string key)
        {
            return Json(await _friendLinkService.FindEntityAsync(c => c.Id == key));
        }

        [Description("删除友情链接信息")]
        [HttpPost]
        public async Task<IActionResult> Delete(string key)
        {
            return Json(await _friendLinkService.UpdateRemoveCacheAsync(f => new FriendLink() { DeleteMark = true }, c => c.Id == key));
        }
    }
}