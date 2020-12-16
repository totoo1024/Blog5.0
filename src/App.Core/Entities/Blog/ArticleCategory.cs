using System;

namespace App.Core.Entities.Blog
{
    /// <summary>
    /// 文章所属栏目信息记录
    /// </summary>
    [Serializable]
    public class ArticleCategory : Entity<string>
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleId { get; set; }

        /// <summary>
        /// 栏目ID
        /// </summary>
        public string CategoryId { get; set; }
    }
}
