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
    class FriendLinkLogic : BaseLogic<FriendLink>, IFriendLinkLogic
    {
        IFriendLinkRepository _friendLinkRepository;
        public FriendLinkLogic(IFriendLinkRepository friendLinkRepository) : base(friendLinkRepository)
        {
            _friendLinkRepository = friendLinkRepository;
        }

        /// <summary>
        /// 新增/编辑友情链接
        /// </summary>
        /// <param name="link">友情链接信息</param>
        /// <returns></returns>
        public OperateResult Save(FriendLink link)
        {
            if (string.IsNullOrWhiteSpace(link.FriendLinkId))
            {
                link.FriendLinkId = SnowflakeUtil.NextStringId();
                return InsertRemoveCache(link);
            }
            else
            {
                return UpdateRemoveCache(link, f => new { f.DeleteMark, f.CreatorTime });
            }
        }
    }
}
