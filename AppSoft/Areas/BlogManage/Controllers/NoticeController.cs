using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Entities;
using App.IServices;
using App.Entities.Dtos;
using System.ComponentModel;
using AppSoft.Filter;

namespace AppSoft.Areas.BlogManage.Controllers
{
    [Area("BlogManage")]
    public class NoticeController : BaseControler
    {
        private readonly INoticeinfoLogic _noticeinfoLogic;
        public NoticeController(INoticeinfoLogic noticeinfoLogic)
        {
            _noticeinfoLogic = noticeinfoLogic;
        }

        [HttpPost]
        [Description("通知列表")]
        public IActionResult Index(QueryDto query)
        {
            return Json(_noticeinfoLogic.QueryableByPage(query), "yyyy-MM-dd HH:mm:ss");
        }

        [HttpPost]
        [Description("新增/编辑通知")]
        public IActionResult Form(Noticeinfo noticeinfo)
        {
            return Json(_noticeinfoLogic.Save(noticeinfo));
        }
        
        [AllowAccessFilter]
        [Description("通知详情")]
        public IActionResult Detail(string key)
        {
            return Json(_noticeinfoLogic.FindEntity(c => c.NoticeId == key));
        }

        [HttpPost]
        [Description("删除通知")]
        public IActionResult Delete(string key)
        {
            return Json(_noticeinfoLogic.Update(n => new Noticeinfo() { DeleteMark = true }, c => c.NoticeId == key));
        }
    }
}