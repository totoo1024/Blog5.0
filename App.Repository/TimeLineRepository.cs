using System;
using System.Collections.Generic;
using System.Text;
using App.Core;
using App.Entities;
using App.IRepository;
using SqlSugar;

namespace App.Repository
{
    public class TimeLineRepository : BaseRepository<TimeLine>, ITimeLineRepository
    {
        public TimeLineRepository()
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
