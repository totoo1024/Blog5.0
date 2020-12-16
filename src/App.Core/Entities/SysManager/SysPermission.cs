using System;
using SqlSugar;

namespace App.Core.Entities.SysManager
{
    /// <summary>
    /// 角色授权信息
    /// </summary>
    [Serializable]
    public class SysPermission : Entity<string>
    {
        /// <summary>
        /// 模块类型1-模块2-按钮
        /// </summary>
        public int? ModuleType { get; set; }
        /// <summary>
        /// 对应模块ID（SysModule主键Id或SysButton主键Id）
        /// </summary>
        public string SysModuleId { get; set; }
        /// <summary>
        /// 对象分类1-角色2-部门-3用户（默认1；其他值扩展使用）
        /// </summary>
        public int? ObjectType { get; set; }
        /// <summary>
        /// 授权的角色/部门/用户Id
        /// </summary>
        public string AuthorizeId { get; set; }
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
