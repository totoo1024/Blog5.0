using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.IRepository;
using App.Core;
using SqlSugar;

namespace App.Repository
{
    public class NoticeInfoRepository : BaseRepository<Noticeinfo>, INoticeInfoRepository
    {
        public NoticeInfoRepository()
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
