using System;
using System.Collections.Generic;
using System.Text;
using App.IRepository;
using App.Entities;
using App.IServices;
using App.Entities.Dtos;
using App.Common.Utils;

namespace App.Services
{
    public class BannerInfoLogic : BaseLogic<BannerInfo>, IBannerInfoLogic
    {
        IBannerInfoRepository _bannerInfoRepository;
        public BannerInfoLogic(IBannerInfoRepository bannerInfoRepository) : base(bannerInfoRepository)
        {
            _bannerInfoRepository = bannerInfoRepository;
        }

        /// <summary>
        /// 添加/修改banner图
        /// </summary>
        /// <param name="banner">轮播图信息</param>
        /// <returns></returns>
        public OperateResult Save(BannerInfo banner)
        {
            if (string.IsNullOrWhiteSpace(banner.BannerId))
            {
                banner.BannerId = SnowflakeUtil.NextStringId();
                return InsertRemoveCache(banner);
            }
            else
            {
                return UpdateRemoveCache(banner, i => new { i.CreatorTime, i.DeleteMark });
            }
        }
    }
}
