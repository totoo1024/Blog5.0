using SqlSugar;
using System;

namespace App.Entities
{
    /// <summary>
    /// 文章栏目分类
    /// </summary>
    [Serializable]
    public class CategoryInfo
    {
        /// <summary>
        /// 栏目ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string CategoryId { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 父级栏目ID
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortCode { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool EnabledMark { get; set; } = true;

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool DeleteMark { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatorTime { get; set; } = DateTime.Now;
    }
}
