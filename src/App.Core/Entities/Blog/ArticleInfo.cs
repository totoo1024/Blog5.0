using System;
using System.Collections.Generic;
using App.Core.Data;
using App.Core.Share;
using SqlSugar;

namespace App.Core.Entities.Blog
{
    /// <summary>
    /// 记录文章信息
    /// </summary>
    [Serializable]
    public class ArticleInfo : Entity<string>, ISoftDelete
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 创作类型：0-原创；1-转载
        /// </summary>
        public CreativeType CreativeType { get; set; }

        /// <summary>
        /// 文章来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 来源链接
        /// </summary>
        public string SourceLink { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 内容摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PublishDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool DeleteMark { get; set; }

        /// <summary>
        /// 阅读次数
        /// </summary>
        public int ReadTimes { get; set; } = 0;

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatorTime { get; set; } = DateTime.Now;
    }
}
