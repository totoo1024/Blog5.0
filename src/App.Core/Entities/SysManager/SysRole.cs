using System;
using App.Core.Data;
using SqlSugar;

namespace App.Core.Entities.SysManager
{
    /// <summary>
    /// 系统角色信息
    /// </summary>
    [Serializable]
    public class SysRole : Entity<string>, ISoftDelete
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string EnCode { get; set; }
        /// <summary>
        /// 排序码
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
