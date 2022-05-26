using System;
using App.Core.Data;
using SqlSugar;

namespace App.Core.Entities.SysManager
{
    /// <summary>
    /// 系统菜单基本信息
    /// </summary>
    [Serializable]
    public class SysModule : Entity<string>, ISoftDelete
    {
        /// <summary>
        /// 上级ID（跟节点为0）
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 菜单编码
        /// </summary>
        public string EnCode { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string UrlAddress { get; set; }
        /// <summary>
        /// 打开方式（null：框架页；_blank：新窗口打开）
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpand { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? SortCode { get; set; }
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
        /// 创建人
        /// </summary>
        public string CreatorAccountId { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreatorTime { get; set; } = DateTime.Now;
    }
}
