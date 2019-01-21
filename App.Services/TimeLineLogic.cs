using System;
using System.Collections.Generic;
using System.Text;
using App.Common.Utils;
using App.Entities;
using App.IRepository;
using App.IServices;
using App.Entities.Dtos;

namespace App.Services
{
    public class TimeLineLogic : BaseLogic<TimeLine>, ITimeLineLogic
    {
        ITimeLineRepository _timeLineRepository;
        public TimeLineLogic(ITimeLineRepository timeLineRepository) : base(timeLineRepository)
        {
            _timeLineRepository = timeLineRepository;
        }

        /// <summary>
        /// 添加/编辑时间轴
        /// </summary>
        /// <param name="timeLine"></param>
        /// <returns></returns>
        public OperateResult Save(TimeLine timeLine)
        {
            if (string.IsNullOrWhiteSpace(timeLine.TimeLineId))
            {
                timeLine.TimeLineId = SnowflakeUtil.NextStringId();
                return Insert(timeLine);
            }
            else
            {
                return Update(timeLine, f => new { f.DeleteMark, f.CreatorTime });
            }
        }
    }
}
