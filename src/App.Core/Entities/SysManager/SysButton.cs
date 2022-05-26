using System;
using SqlSugar;

namespace App.Core.Entities.SysManager
{
    /// <summary>
	/// 页面按钮信息
	/// </summary>
    [Serializable]
    public class SysButton : Entity<string>
    {
        /// <summary>
        /// 模块Id（对应SysModule表主键）
        /// </summary>
        public string SysModuleId { get; set; }
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 按钮图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 按钮编码
        /// </summary>
        public string EnCode { get; set; }
        /// <summary>
        /// 按钮位置（1：工具栏；2：表格栏；3：数据列）
        /// </summary>
        public int? Location { get; set; }
        /// <summary>
        /// JS事件
        /// </summary>
        public string JsEvent { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string UrlAddress { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? SortCode { get; set; }
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
