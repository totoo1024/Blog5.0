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
    public class TimeLineController : BaseControler
    {
        private readonly ITimeLineLogic _timeLineLogic;
        public TimeLineController(ITimeLineLogic timeLineLogic)
        {
            _timeLineLogic = timeLineLogic;
        }

        [Description("时光轴列表")]
        [HttpPost]
        public IActionResult Index(QueryDto query)
        {
            return Json(_timeLineLogic.QueryableByPage(query), "yyyy-MM-dd HH:mm:ss");
        }

        [Description("添加/修改时光轴")]
        [HttpPost]
        public IActionResult Form(TimeLine timeLine)
        {
            return Json(_timeLineLogic.Save(timeLine));
        }

        [AllowAccessFilter]
        [Description("时光轴详情")]
        public IActionResult Detail(string key)
        {
            return Json(_timeLineLogic.FindEntity(key), "yyyy-MM-dd HH:mm:ss");
        }

        [Description("删除时光轴")]
        public IActionResult Delete(string key)
        {
            return Json(_timeLineLogic.Update(f => new TimeLine() { DeleteMark = true }, c => c.TimeLineId == key));
        }
    }
}