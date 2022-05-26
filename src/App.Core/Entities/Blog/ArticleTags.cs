using System;
using SqlSugar;

namespace App.Core.Entities.Blog
{
    /// <summary>
    /// 文章所标签
    /// </summary>
    [Serializable]
    public class ArticleTags : Entity<string>
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleId { get; set; }

        /// <summary>
        /// 标签ID
        /// </summary>
        public string TagsId { get; set; }
    }
}
