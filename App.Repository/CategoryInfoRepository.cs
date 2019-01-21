using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.Core;
using App.IRepository;
using SqlSugar;

namespace App.Repository
{
    public class CategoryInfoRepository : BaseRepository<CategoryInfo>, ICategoryInfoRepository
    {
        public CategoryInfoRepository()
        {
            //单表过滤数据
            Db.QueryFilter.Add(new SqlFilterItem
            {
                FilterValue = filete => new SqlFilterResult() { Sql = "DeleteMark=0 " },
                IsJoinQuery = false
            });
        }
    }
}
