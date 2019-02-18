using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Entities;
using App.Entities.Dtos;
using App.IServices;
using AppSoft.Filter;
using System.ComponentModel;

namespace AppSoft.Areas.BlogManage.Controllers
{
    [Area("BlogManage")]
    public class FriendLinkController : BaseControler
    {
        private readonly IFriendLinkLogic _friendLinkLogic;
        public FriendLinkController(IFriendLinkLogic friendLinkLogic)
        {
            _friendLinkLogic = friendLinkLogic;
        }
        [HttpPost]
        [Description("友情链接信息列表")]
        public IActionResult Index(QueryDto query)
        {
            return Json(_friendLinkLogic.QueryableByPage(query), "yyyy-MM-dd HH:mm:ss");
        }

        [Description("新增/编辑友情链接信息")]
        [HttpPost]
        public IActionResult Form(FriendLink friendLink)
        {
            return Json(_friendLinkLogic.Save(friendLink));
        }

        [Description("友情链接详情")]
        [AllowAccessFilter]
        public IActionResult Detail(string key)
        {
            return Json(_friendLinkLogic.FindEntity(c => c.FriendLinkId == key));
        }

        [Description("删除友情链接信息")]
        [HttpPost]
        public IActionResult Delete(string key)
        {
            return Json(_friendLinkLogic.UpdateRemoveCache(f => new FriendLink() { DeleteMark = true }, c => c.FriendLinkId == key));
        }
    }
}