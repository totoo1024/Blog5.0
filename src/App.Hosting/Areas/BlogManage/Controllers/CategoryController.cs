using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using App.Application.Blog;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using Microsoft.AspNetCore.Mvc;

namespace App.Hosting.Areas.BlogManage.Controllers
{
    [Area("BlogManage")]
    public class CategoryController : AdminController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAccess]
        [Description("文章栏目列表")]
        public async Task<IActionResult> Categorys()
        {
            List<CategoryInfo> list = await _categoryService.GetListAsync(m => m.DeleteMark == false, o => o.SortCode, false);
            return Json(new { code = 0, msg = "ok", data = list, count = list.Count() }, "yyyy-MM-dd HH:mm:ss");
        }

        [HttpPost]
        [Description("新增/修改文章栏目")]
        public async Task<IActionResult> Form(CategoryInputDto category)
        {
            return Json(await _categoryService.Save(category));
        }

        [AllowAccess]
        [Description("栏目详情")]
        public async Task<IActionResult> Detail(string key)
        {
            return Json(await _categoryService.FindEntityAsync(c => c.Id == key));
        }

        [HttpPost]
        [Description("启用禁用栏目")]
        public IActionResult Enable(string id, bool status)
        {
            return Json(_categoryService.UpdateRemoveCache(m => new CategoryInfo() { EnabledMark = status }, c => c.Id == id));
        }

        [HttpPost]
        [Description("删除栏目")]
        public async Task<IActionResult> Delete(string key)
        {
            return Json(await _categoryService.UpdateRemoveCacheAsync(info => new CategoryInfo() { DeleteMark = true }, c => c.Id == key));
        }

        [Description("栏目树形节点"), AllowAccess]
        public async Task<IActionResult> Tree()
        {
            List<CategoryInfo> categories = await _categoryService.GetListCacheAsync(m => m.DeleteMark == false && m.EnabledMark);
            var tree = GetTree(categories, "0");
            List<object> list = new List<object>() {
                    new{ id="0", name="一级栏目",icon="",spread = true, children =tree}
                };
            return Json(list);
        }

        [AllowAccess]
        [Description("获取所有栏目")]
        public async Task<IActionResult> List()
        {
            List<CategoryInfo> list = await _categoryService.GetListCacheAsync(c => c.EnabledMark == true);
            return Json(TreeJson(list));
        }

        /// <summary>
        /// 获取栏目（递归）
        /// </summary>
        /// <param name="menus">所有栏目</param>
        /// <param name="pid">父级id</param>
        /// <returns></returns>
        private List<object> GetTree(List<CategoryInfo> categories, string pid)
        {
            List<object> tree = new List<object>();
            var list = categories.Where(m => m.ParentId == pid).OrderBy(o => o.SortCode);
            foreach (var item in list)
            {
                var child = GetTree(categories, item.Id);
                tree.Add(new { id = item.Id, name = item.CategoryName, icon = "", spread = true, children = child });
            }
            return tree;
        }
        /// <summary>
        /// 栏目树
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        private List<object> TreeJson(List<CategoryInfo> categories, string pid = "0")
        {
            List<object> list = new List<object>();
            foreach (var item in categories.Where(c => c.ParentId == pid))
            {
                List<object> childs = TreeJson(categories, item.Id);
                list.Add(new { name = item.CategoryName, value = item.Id, children = childs });
            }
            return list;
        }
    }
}