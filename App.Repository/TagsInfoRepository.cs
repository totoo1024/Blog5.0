using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.IRepository;
using App.Core;
using SqlSugar;

namespace App.Repository
{
    public class TagsInfoRepository : BaseRepository<TagsInfo>, ITagsInfoRepository
    {
        public TagsInfoRepository()
        {
            //单表过滤数据
            Db.QueryFilter.Add(new SqlFilterItem
            {
                FilterValue = filete => new SqlFilterResult() { Sql = "DeleteMark=0 " },
                IsJoinQuery = false
            });
        }

        /// <summary>
        /// 查询各个标签文章数量
        /// </summary>
        /// <returns></returns>
        public dynamic TagsCount()
        {
            return Db.Queryable<TagsInfo>().Where(tag => tag.EnabledMark == true)
                  .OrderBy(o => o.SortCode, OrderByType.Asc)
                  .Select(tag => new
                  {
                      TagId = tag.TagId,
                      TagName = tag.TagName,
                      Color = tag.BGColor,
                      Total = SqlFunc.Subqueryable<ArticleTags>().Where(at => SqlFunc.Subqueryable<ArticleInfo>()
                      .Where(c => tag.TagId == at.TagsId && c.ArticleId == at.ArticleId && c.DeleteMark == false && c.Visible == true).Any()
                  ).Select(s => SqlFunc.AggregateCount(s.ArticleId))
                  }).ToList();
        }
    }
}
