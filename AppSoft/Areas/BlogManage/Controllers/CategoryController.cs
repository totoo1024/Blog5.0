using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Entities.Dtos;
using App.Entities;
using App.IServices;
using System.ComponentModel;
using AppSoft.Filter;

namespace AppSoft.Areas.BlogManage.Controllers
{
    [Area("BlogManage")]
    public class CategoryController : BaseControler
    {
        private readonly ICategoryInfoLogic _categoryInfoLogic;
        public CategoryController(ICategoryInfoLogic categoryInfoLogic)
        {
            _categoryInfoLogic = categoryInfoLogic;
        }

        [AllowAccessFilter]
        [Description("文章栏目列表")]
        public IActionResult Categorys()
        {
            List<CategoryInfo> list = _categoryInfoLogic.Queryable(m => m.DeleteMark == false, o => o.SortCode, false);
            return Json(new { code = 0, msg = "ok", data = list, count = list.Count() }, "yyyy-MM-dd HH:mm:ss");
        }

        [HttpPost]
        [Description("新增/修改文章栏目")]
        public IActionResult Form(CategoryInfo category)
        {
            return Json(_categoryInfoLogic.Save(category));
        }

        [AllowAccessFilter]
        [Description("栏目详情")]
        public IActionResult Detail(string key)
        {
            return Json(_categoryInfoLogic.FindEntity(c => c.CategoryId == key));
        }

        [HttpPost]
        [Description("启用禁用栏目")]
        public IActionResult Enable(string id, bool status)
        {
            return Json(_categoryInfoLogic.Update(m => new CategoryInfo() { EnabledMark = status }, c => c.CategoryId == id));
        }

        [HttpPost]
        [Description("删除栏目")]
        public IActionResult Delete(string key)
        {
            return Json(_categoryInfoLogic.Update(info => new CategoryInfo() { DeleteMark = true }, c => c.CategoryId == key));
        }

        [Description("栏目树形节点"), AllowAccessFilter]
        public IActionResult Tree()
        {
            List<CategoryInfo> categories = _categoryInfoLogic.Queryable(m => m.DeleteMark == false && m.EnabledMark == true);
            var tree = GetTree(categories, "0");
            List<object> list = new List<object>() {
                    new{ id="0", name="一级栏目",icon="",spread = true, children =tree}
                };
            return Json(list);
        }

        [AllowAccessFilter]
        [Description("获取所有栏目")]
        public IActionResult List()
        {
            List<CategoryInfo> list = _categoryInfoLogic.Queryable(c => c.EnabledMark == true);
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
                var child = GetTree(categories, item.CategoryId);
                tree.Add(new { id = item.CategoryId, name = item.CategoryName, icon = "", spread = true, children = child });
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
                List<object> childs = TreeJson(categories, item.CategoryId);
                list.Add(new { name = item.CategoryName, value = item.CategoryId, children = childs });
            }
            return list;
        }
    }
}