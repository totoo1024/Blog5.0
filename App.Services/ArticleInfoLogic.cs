using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.IRepository;
using App.IServices;
using App.Common.Utils;
using System.Linq;
using App.Entities.Dtos;

namespace App.Services
{
    public class ArticleInfoLogic : BaseLogic<ArticleInfo>, IArticleInfoLogic
    {
        IArticleInfoRepository _articleInfoRepository;
        public ArticleInfoLogic(IArticleInfoRepository articleInfoRepository) : base(articleInfoRepository)
        {
            _articleInfoRepository = articleInfoRepository;
        }

        /// <summary>
        /// 添加/编辑文章
        /// </summary>
        /// <param name="article">文章信息</param>
        /// <returns></returns>
        public OperateResult Save(ArticleInfo article)
        {
            bool b = string.IsNullOrWhiteSpace(article.ArticleId);
            if (b)
            {
                article.ArticleId = SnowflakeUtil.NextStringId();
            }
            if (!article.Categories.Any())
            {
                return new OperateResult("请选择文章分类");
            }
            if (!article.Tags.Any())
            {
                return new OperateResult("请选择文章标签");
            }
            List<ArticleCategory> categories = new List<ArticleCategory>();
            List<ArticleTags> tags = new List<ArticleTags>();
            article.Categories.ForEach(c =>
            {
                categories.Add(new ArticleCategory() { ACId = SnowflakeUtil.NextStringId(), ArticleId = article.ArticleId, CategoryId = c });
            });
            article.Tags.ForEach(t =>
            {
                tags.Add(new ArticleTags() { ArticleTagsId = SnowflakeUtil.NextStringId(), ArticleId = article.ArticleId, TagsId = t });
            });
            _articleInfoRepository.Save(article, categories, tags, b);
            return new OperateResult("保存成功", ResultStatus.Success);
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
        public PageResult<List<ArticleDto>> ArticleList(string key, string id, int type, string sort, int pageIndex, int pageSize)
        {
            PageResult<List<ArticleDto>> result = new PageResult<List<ArticleDto>>();
            var tuple = _articleInfoRepository.ArticleList(key, id, type, sort, pageIndex, pageSize);
            result.code = 0;
            result.count = tuple.Item2;
            result.data = tuple.Item1;
            return result;
        }
    }
}
