using SqlSugar;
using System;
namespace App.Entities
{
    /// <summary>
    /// 文章所属栏目信息记录
    /// </summary>
    [Serializable]
    public class ArticleCategory
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string ACId { get; set; }

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
