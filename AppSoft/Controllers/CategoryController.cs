using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Entities;
using App.IServices;

namespace AppSoft.Controllers
{
    public class CategoryController : BaseWebController
    {
        private readonly ICategoryInfoLogic _categoryInfoLogic;
        public CategoryController(ICategoryInfoLogic categoryInfoLogic)
        {
            _categoryInfoLogic = categoryInfoLogic;
        }
        public IActionResult Index()
        {
            return Json(_categoryInfoLogic.QueryableCache(c => c.EnabledMark == true && c.ParentId == "0").Select(s => new { CategoryId = s.CategoryId, CategoryName = s.CategoryName }));
        }
    }
}