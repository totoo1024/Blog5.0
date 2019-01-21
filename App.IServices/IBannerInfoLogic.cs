using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.Entities.Dtos;

namespace App.IServices
{
    public interface IBannerInfoLogic : IBaseLogic<BannerInfo>
    {
        /// <summary>
        /// 新增、编辑轮播图信息
        /// </summary>
        /// <param name="banner">轮播图信息</param>
        /// <returns></returns>
        OperateResult Save(BannerInfo banner);
    }
}
