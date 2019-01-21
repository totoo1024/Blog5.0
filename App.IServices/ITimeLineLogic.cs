using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.Entities.Dtos;

namespace App.IServices
{
    public interface ITimeLineLogic : IBaseLogic<TimeLine>
    {
        /// <summary>
        /// 添加/编辑时间轴
        /// </summary>
        /// <param name="timeLine"></param>
        /// <returns></returns>
        OperateResult Save(TimeLine timeLine);
    }
}
