using System;
using System.Collections.Generic;
using System.Text;
using App.Core;
using App.Entities;

namespace App.IRepository
{
    public interface ITagsInfoRepository : IBaseRepository<TagsInfo>
    {
        /// <summary>
        /// 查询各个标签文章数量
        /// </summary>
        /// <returns></returns>
        dynamic TagsCount();
    }
}
