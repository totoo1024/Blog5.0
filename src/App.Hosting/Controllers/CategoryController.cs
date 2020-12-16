using System.Threading.Tasks;
using App.Application.Blog;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace App.Hosting.Controllers
{
    public class CategoryController : WebController
    {
        private readonly ICategoryService _categoryInfoLogic;
        public CategoryController(ICategoryService categoryInfoLogic)
        {
            _categoryInfoLogic = categoryInfoLogic;
        }
        public async Task<IActionResult> Index()
        {
            return Json((await _categoryInfoLogic.GetListAsync(c => c.EnabledMark && c.ParentId == "0")).Select(s => new { CategoryId = s.Id, CategoryName = s.CategoryName }));
        }
    }
}