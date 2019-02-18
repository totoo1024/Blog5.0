using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entities;
using App.IRepository;
using App.IServices;
using App.Common.Utils;
using App.Entities.Dtos;

namespace App.Services
{
    public class SysModuleLogic : BaseLogic<SysModule>, ISysModuleLogic
    {
        private ISysModuleRepository _sysModuleRepository;
        private ISysButtonLogic _sysButtonLogic;

        public SysModuleLogic(ISysModuleRepository sysModuleRepository, ISysButtonLogic sysButtonLogic) : base(sysModuleRepository)
        {
            _sysModuleRepository = sysModuleRepository;
            _sysButtonLogic = sysButtonLogic;
        }

        /// <summary>
        /// 新增/修改菜单
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public OperateResult Save(SysModule module)
        {
            if (QueryableCount(m => m.EnCode.ToLower() == module.EnCode.ToLower() && m.ModuleId != module.ModuleId) > 0)
            {
                OperateResult result = new OperateResult();
                result.Message = "菜单编码已存在";
                return result;
            }
            else
            {
                module.ParentId = string.IsNullOrWhiteSpace(module.ParentId) ? null : module.ParentId;
                if (string.IsNullOrWhiteSpace(module.ModuleId))
                {
                    module.ModuleId = SnowflakeUtil.NextStringId();
                    return InsertRemoveCache(module);
                }
                else
                {
                    if (module.ModuleId == module.ParentId)
                    {
                        OperateResult result = new OperateResult();
                        result.Message = "父级菜单不能指向当前菜单";
                        return result;
                    }
                    if (module.ParentId != "0")
                    {
                        var list = QueryableCache(m => m.DeleteMark == false && m.EnabledMark == true);
                        if (GetChildMenu(list, module.ModuleId).Where(o => o.ModuleId == module.ParentId).Any())
                        {
                            OperateResult result = new OperateResult();
                            result.Message = "无法将父级菜单指定到子级菜单下";
                            return result;
                        }
                    }
                    return UpdateRemoveCache(module, c => new { c.CreatorTime, c.CreatorAccountId, c.DeleteMark });
                }
            }
        }

        /// <summary>
        /// 菜单按钮树
        /// </summary>
        /// <returns></returns>
        public async Task<List<TreeModuleDto>> Tree()
        {
            //所有菜单
            var menuList = await QueryableCacheAsync(m => m.EnabledMark == true && m.DeleteMark == false);
            //所有按钮
            var buttonList = await _sysButtonLogic.QueryableCacheAsync();

            var list = GetTrees(menuList, buttonList, "0");
            //list.Insert(0, new TreeModule() { id = Guid.Empty.ToString(), name = "模块", pid = "" });
            return list;
        }


        /// <summary>
        /// 获取菜单按钮树
        /// </summary>
        /// <param name="modules">所有菜单</param>
        /// <param name="buttons">所有按钮</param>
        /// <param name="parentId">上级id</param>
        /// <returns></returns>
        private List<TreeModuleDto> GetTrees(List<SysModule> modules, List<SysButton> buttons, string parentId)
        {
            List<TreeModuleDto> treeModules = new List<TreeModuleDto>();
            foreach (var item in modules.Where(m => m.ParentId == parentId))
            {
                TreeModuleDto tree = new TreeModuleDto()
                {
                    id = item.ModuleId,
                    name = item.FullName,
                    pid = item.ParentId == null ? "0" : item.ParentId.ToString()
                };
                treeModules.Add(tree);

                var bts = buttons.Where(b => b.SysModuleId == item.ModuleId);//获取当前菜单下所有按钮
                if (bts.Any())
                {
                    var trees = from b in bts select new TreeModuleDto() { id = b.ButtonId, name = b.FullName, pid = b.SysModuleId.ToString() };
                    treeModules.AddRange(trees);
                }
                treeModules.AddRange(GetTrees(modules, buttons, item.ModuleId));
            }
            return treeModules;
        }

        /// <summary>
        /// 获取指定菜单下所有菜单
        /// </summary>
        /// <param name="list">所有菜单集合</param>
        /// <param name="id">指定菜单ID</param>
        /// <returns></returns>
        private List<SysModule> GetChildMenu(List<SysModule> list, string id)
        {
            List<SysModule> modules = new List<SysModule>();
            foreach (var item in list.Where(m => m.ParentId == id))
            {
                modules.Add(item);
                GetChildMenu(list, item.ModuleId);
            }
            return modules;
        }
    }
}
