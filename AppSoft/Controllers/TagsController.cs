using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.IServices;
using System.ComponentModel;

namespace AppSoft.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagsInfoLogic _tagsInfoLogic;
        public TagsController(ITagsInfoLogic tagsInfoLogic)
        {
            _tagsInfoLogic = tagsInfoLogic;
        }

        [Description("文章所属标签数量统计")]
        public IActionResult Index()
        {
            return Json(_tagsInfoLogic.TagsCount());
        }
    }
}