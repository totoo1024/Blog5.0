using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Entities.Dtos;
using App.IServices;
using App.Entities;
using SqlSugar;

namespace AppSoft.Controllers
{
    public class ArticleController : BaseWebController
    {
        private readonly IArticleInfoLogic _articleInfoLogic;
        private readonly ICategoryInfoLogic _categoryInfoLogic;
        private readonly ITagsInfoLogic _tagsInfoLogic;
        public ArticleController(IArticleInfoLogic articleInfoLogic, ICategoryInfoLogic categoryInfoLogic, ITagsInfoLogic tagsInfoLogic)
        {
            _articleInfoLogic = articleInfoLogic;
            _categoryInfoLogic = categoryInfoLogic;
            _tagsInfoLogic = tagsInfoLogic;
        }

        /// <summary>
        /// 文章专栏页
        /// </summary>
        /// <param name="cid">栏目id</param>
        /// <param name="tid">标签id</param>
        /// <returns></returns>
        public IActionResult List(string cid, string tid)
        {
            string name = "";
            if (!string.IsNullOrWhiteSpace(cid))
            {
                name = _categoryInfoLogic.FindEntity(c => c.EnabledMark == true && c.CategoryId == cid)?.CategoryName;
            }
            if (!string.IsNullOrWhiteSpace(tid))
            {
                name = _tagsInfoLogic.FindEntity(c => c.EnabledMark == true && c.TagId == tid)?.TagName;
            }
            ViewBag.CategoryName = name;
            return View();
        }

        /// <summary>
        /// 首页文章
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="limit">每页显示数量</param>
        /// <returns></returns>
        public IActionResult Page(int page = 1, int limit = 10)
        {
            var data = _articleInfoLogic.ArticleList("", "", 1, "IsTop desc,PublishDate desc", page, limit);
            if (data.count > 0)
            {
                var no = data.count * 1d / limit;
                data.count = (int)Math.Ceiling(no);
            }
            return Json(data, "yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 文章专栏列表
        /// </summary>
        /// <param name="key">关键词</param>
        /// <param name="tid">标签id</param>
        /// <param name="cid">栏目id</param>
        /// <param name="page">当前页</param>
        /// <param name="limit">每页显示的条数</param>
        /// <returns></returns>
        public IActionResult Views(string key, string tid, string cid, int page = 1, int limit = 10)
        {
            int type = 1;
            string id = "";
            id = tid ?? id;
            id = cid ?? id;
            if (!string.IsNullOrWhiteSpace(tid))
            {
                type = 2;
            }
            var data = _articleInfoLogic.ArticleList(key, id, type, "IsTop desc,PublishDate desc", page, limit);
            if (data.count > 0)
            {
                var no = data.count * 1d / limit;
                data.count = (int)Math.Ceiling(no);
            }
            return Json(data, "yyyy-MM-dd");
        }

        /// <summary>
        /// 热门文章
        /// </summary>
        /// <returns></returns>
        public IActionResult Hot()
        {
            var list = _articleInfoLogic.Queryable(c => c.Visible == true, o => o.ReadTimes, true, 6);
            return Json(list.Select(s => new { ArticleId = s.ArticleId, Title = s.Title, Thumbnail = s.Thumbnail, PublishDate = s.PublishDate, ReadTimes = s.ReadTimes }), "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 文章详情
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public IActionResult Detail(string id)
        {
            ArticleInfo article = _articleInfoLogic.FindEntity(c => c.Visible == true && c.ArticleId == id);
            if (article != null)
            {
                _articleInfoLogic.Update(f => new ArticleInfo() { ReadTimes = f.ReadTimes + 1 }, c => c.ArticleId == id);
            }
            return View(article);
        }

        /// <summary>
        /// 随机文章10条
        /// </summary>
        /// <returns></returns>
        public IActionResult Random()
        {
            return Json(_articleInfoLogic.Queryable(c => c.Visible == true, o => SqlFunc.GetRandom(), false, 10).Select(s => new { ArticleId = s.ArticleId, Title = s.Title }));
        }
    }
}