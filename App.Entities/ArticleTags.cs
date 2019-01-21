using SqlSugar;
using System;

namespace App.Entities
{
    /// <summary>
    /// 文章所标签
    /// </summary>
    [Serializable]
    public class ArticleTags
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string ArticleTagsId { get; set; }

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
