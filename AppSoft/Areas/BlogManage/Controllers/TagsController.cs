using System;
using Microsoft.AspNetCore.Mvc;
using App.Entities;
using App.Entities.Dtos;
using App.IServices;
using System.ComponentModel;
using AppSoft.Filter;
using System.Linq;

namespace AppSoft.Areas.BlogManage.Controllers
{
    [Area("BlogManage")]
    public class TagsController : BaseControler
    {
        private readonly ITagsInfoLogic _tagsInfoLogic;
        public TagsController(ITagsInfoLogic tagsInfoLogic)
        {
            _tagsInfoLogic = tagsInfoLogic;
        }

        [HttpPost]
        [Description("标签列表")]
        public IActionResult Index(QueryDto query)
        {
            return Json(_tagsInfoLogic.QueryableByPage(query), "yyyy-MM-dd HH:mm:ss");
        }

        [HttpPost]
        [Description("新增/编辑标签")]
        public IActionResult Form(TagsInfo tags)
        {
            return Json(_tagsInfoLogic.Save(tags));
        }

        [AllowAccessFilter]
        [Description("标签详情")]
        public IActionResult Detail(string key)
        {
            return Json(_tagsInfoLogic.FindEntity(c => c.TagId == key));
        }

        [HttpPost]
        [Description("删除标签")]
        public IActionResult Delete(string key)
        {
            return Json(_tagsInfoLogic.Update(tag => new TagsInfo() { DeleteMark = true }, c => c.TagId == key));
        }

        [HttpPost]
        [Description("启用禁用标签")]
        public IActionResult Enable(string id, bool status)
        {
            return Json(_tagsInfoLogic.Update(tag => new TagsInfo() { EnabledMark = status }, c => c.TagId == id));
        }

        [AllowAccessFilter]
        [Description("获取所有标签")]
        public IActionResult List()
        {
            return Json(_tagsInfoLogic.Queryable(c => c.EnabledMark == true).Select(d => new { value = d.TagId, name = d.TagName, selected = "", disabled = "" }));
        }
    }
}