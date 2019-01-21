using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.Entities.Dtos;

namespace App.IServices
{
    public interface INoticeinfoLogic : IBaseLogic<Noticeinfo>
    {
        /// <summary>
        /// 添加/编辑通知
        /// </summary>
        /// <param name="noticeinfo">通知信息</param>
        /// <returns></returns>
        OperateResult Save(Noticeinfo noticeinfo);
    }
}
