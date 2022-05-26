using System;
using App.Core.Data;
using SqlSugar;

namespace App.Core.Entities.Blog
{
    /// <summary>
    /// 文章相关标签
    /// </summary>
    [Serializable]
    public class TagsInfo : Entity<string>, ISoftDelete
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public string BGColor { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public sbyte SortCode { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool DeleteMark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool EnabledMark { get; set; } = true;

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatorTime { get; set; } = DateTime.Now;
    }
}
