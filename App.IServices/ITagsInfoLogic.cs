using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.Entities.Dtos;

namespace App.IServices
{
    public interface ITagsInfoLogic : IBaseLogic<TagsInfo>
    {
        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="tags">标签信息</param>
        /// <returns></returns>
        OperateResult Save(TagsInfo tags);

        /// <summary>
        /// 查询各个标签文章数量
        /// </summary>
        /// <returns></returns>
        dynamic TagsCount();
    }
}
