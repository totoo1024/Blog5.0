using System;
using System.Collections.Generic;
using System.Text;
using App.Core;
using App.Entities;
using App.Entities.Dtos;

namespace App.IRepository
{
    public interface IArticleInfoRepository : IBaseRepository<ArticleInfo>
    {
        /// <summary>
        /// 添加/编辑文章
        /// </summary>
        /// <param name="article">文章信息</param>
        /// <param name="articleCategorys">文章所属分类</param>
        /// <param name="articleTags">文章所属标签</param>
        /// <param name="isAdd">是否是添加</param>
        /// <returns></returns>
        bool Save(ArticleInfo article, List<ArticleCategory> articleCategorys, List<ArticleTags> articleTags, bool isAdd);

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
        Tuple<List<ArticleDto>, int> ArticleList(string key, string id, int type, string sort, int pageIndex, int pageSize);
    }
}
