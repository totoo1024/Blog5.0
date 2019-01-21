using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Entities;
using App.Entities.Dtos;
using App.IServices;
using System.ComponentModel;

namespace AppSoft.Areas.BlogManage.Controllers
{
    [Area("BlogManage")]
    public class QQUserController : BaseControler
    {
        private readonly IQQUserinfoLogic _qQUserinfoLogic;
        public QQUserController(IQQUserinfoLogic qQUserinfoLogic)
        {
            _qQUserinfoLogic = qQUserinfoLogic;
        }

        [HttpPost]
        [Description("QQ用户列表")]
        public IActionResult Index(QueryDto query)
        {
            return Json(_qQUserinfoLogic.QueryableByPage(query), "yyyy-MM-dd HH:mm:ss");
        }

        [HttpPost]
        [Description("是否设置为博主")]
        public IActionResult Master(string key, bool status)
        {
            return Json(_qQUserinfoLogic.Update(qq => new QQUserinfo() { IsMaster = status }, c => c.UserId == key));
        }

        [HttpPost]
        [Description("删除QQ用户")]
        public IActionResult Delete(string key)
        {
            return Json(_qQUserinfoLogic.Delete(c => c.UserId == key));
        }
    }
}