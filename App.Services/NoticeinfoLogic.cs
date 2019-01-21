using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.IServices;
using App.IRepository;
using App.Entities.Dtos;
using App.Common.Utils;

namespace App.Services
{
    public class NoticeinfoLogic : BaseLogic<Noticeinfo>, INoticeinfoLogic
    {
        INoticeInfoRepository _noticeInfoRepository;
        public NoticeinfoLogic(INoticeInfoRepository noticeInfoRepository) : base(noticeInfoRepository)
        {
            _noticeInfoRepository = noticeInfoRepository;
        }

        /// <summary>
        /// 添加/编辑通知
        /// </summary>
        /// <param name="noticeinfo">通知信息</param>
        /// <returns></returns>
        public OperateResult Save(Noticeinfo noticeinfo)
        {
            if (string.IsNullOrWhiteSpace(noticeinfo.NoticeId))
            {
                noticeinfo.NoticeId = SnowflakeUtil.NextStringId();
                return Insert(noticeinfo);
            }
            else
            {
                return Update(noticeinfo, f => new { f.DeleteMark, f.CreatorTime });
            }
        }
    }
}
