using System;
using App.Core.Data;
using SqlSugar;

namespace App.Core.Entities.Blog
{
    /// <summary>
    /// 首页Banner图
    /// </summary>
    [Serializable]
    public class BannerInfo : Entity<string>, ISoftDelete
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 跳转方式
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public sbyte? SortCode { get; set; }

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
