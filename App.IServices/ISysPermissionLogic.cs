using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entities;
using App.Entities.Dtos;

namespace App.IServices
{
    public interface ISysPermissionLogic : IBaseLogic<SysPermission>
    {
        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="permission">分配权限菜单按钮id集合</param>
        /// <param name="createUserId">创建人userid</param>
        /// <returns></returns>
        Task<OperateResult> Save(string roleId, List<string> permission, string createUserId);

        /// <summary>
        /// 获取指定角色、部门、用户所有可访问的菜单按钮信息
        /// </summary>
        /// <param name="authorrizeId">角色ID、部门ID、用户ID</param>
        /// <returns></returns>
        Task<object> GetMenuAndButton(params string[] authorrizeId);

        /// <summary>
        /// 检验权限
        /// </summary>
        /// <param name="authorizeId">授权id（角色ID/部门ID/用户ID）</param>
        /// <param name="url">授权地址</param>
        /// <returns></returns>
        bool CheckPermission(string authorizeId, string url);
    }
}
