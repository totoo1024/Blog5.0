using System;
using App.Core.Data;
using SqlSugar;

namespace App.Core.Entities.Blog
{
    /// <summary>
    /// 友情链接
    /// </summary>
    [Serializable]
    public class FriendLink : Entity<string>, ISoftDelete
    {
        /// <summary>
        /// 网站名称
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 网站链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 网站logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public sbyte SortCode { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool DeleteMark { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatorTime { get; set; } = DateTime.Now;
    }
}
