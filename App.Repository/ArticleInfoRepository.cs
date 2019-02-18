using System;
using System.Collections.Generic;
using System.Text;
using App.IRepository;
using App.Entities;
using App.Core;
using SqlSugar;
using App.Entities.Dtos;
using System.Linq;
using System.Linq.Expressions;

namespace App.Repository
{
    public class ArticleInfoRepository : BaseRepository<ArticleInfo>, IArticleInfoRepository
    {
        public ArticleInfoRepository()
        {
            //单表过滤数据
            Db.QueryFilter.Add(new SqlFilterItem
            {
                FilterValue = filete => new SqlFilterResult() { Sql = "DeleteMark=0 " },
                IsJoinQuery = false
            });
        }
        /// <summary>
        /// 添加/编辑文章
        /// </summary>
        /// <param name="article">文章信息</param>
        /// <param name="articleCategorys">文章所属分类</param>
        /// <param name="articleTags">文章所属标签</param>
        /// <param name="isAdd">是否是添加</param>
        /// <returns></returns>
        public bool Save(ArticleInfo article, List<ArticleCategory> articleCategorys, List<ArticleTags> articleTags, bool isAdd)
        {
            //使用事务执行
            return AppDbContext.ExecTran(db =>
             {
                 if (isAdd)
                 {
                     db.Insertable(article).ExecuteCommand();
                     db.Insertable(articleCategorys).ExecuteCommand();
                     db.Insertable(articleTags).ExecuteCommand();
                 }
                 else
                 {
                     db.Updateable(article).IgnoreColumns(a => new { a.CreatorTime, a.ReadTimes }).ExecuteCommand();
                     db.Deleteable<ArticleCategory>(c => c.ArticleId == article.ArticleId).ExecuteCommand();
                     db.Deleteable<ArticleTags>(c => c.ArticleId == article.ArticleId).ExecuteCommand();
                     db.Insertable(articleCategorys).ExecuteCommand();
                     db.Insertable(articleTags).ExecuteCommand();
                 }
                 return true;
             });
        }

        /// <summary>
        /// 文章列表分页
        /// </summary>
        /// <param name="key">关键词</param>
        /// <param name="id">栏目id或表情id</param>
        /// <param name="type">1：栏目id；2：表情id</param>
        /// <param name="sort">排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <returns></returns>
        public Tuple<List<ArticleDto>, int> ArticleList(string key, string id, int type, string sort, int pageIndex, int pageSize)
        {
            int total = 0;
            var query = Db.Queryable<ArticleInfo>().Where(a => a.Visible == true);
            if (!string.IsNullOrEmpty(key))
            {
                query.Where(a => a.Title.Contains(key)
                || SqlFunc.Subqueryable<TagsInfo>().Where(t => t.TagName.ToLower().Contains(key.ToLower())).Any()
                || SqlFunc.Subqueryable<CategoryInfo>().Where(ci => ci.CategoryName.ToLower().Contains(key.ToLower())).Any()
                );

            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (type == 1)
                {
                    query.Where(a => SqlFunc.Subqueryable<ArticleCategory>().Where(ac => ac.ArticleId == a.ArticleId && ac.CategoryId == id).Any());
                }
                else
                {
                    query.Where(a => SqlFunc.Subqueryable<ArticleTags>().Where(at => at.ArticleId == a.ArticleId && at.TagsId == id).Any());
                }
            }
            if (!string.IsNullOrWhiteSpace(sort))
            {
                query.OrderBy(sort);
            }
            List<ArticleDto> ad = query.Select<ArticleDto>().Mapper((it, cache) =>
                 {
                     Db.QueryFilter.Clear();
                     var tids = cache.Get(list =>
                      {
                          var ids = list.Select(i => it.ArticleId).ToList();
                          return Db.Queryable<ArticleTags>().Where(c => ids.Contains(c.ArticleId)).ToList().Select(f => f.TagsId);
                      });

                     it.Tags = Db.Queryable<TagsInfo>().Where(c => c.DeleteMark == false && c.EnabledMark == true && tids.Contains(c.TagId)).Select<TagDto>().ToList();
                     it.MsgTimes = Db.Queryable<LeavemsgInfo>().Where(msg => SqlFunc.IsNullOrEmpty(msg.ToUId) && msg.ArticleId == it.ArticleId).Count();

                 }).ToPageList(pageIndex, pageSize, ref total);
            return Tuple.Create(ad, total);
        }
    }
}
