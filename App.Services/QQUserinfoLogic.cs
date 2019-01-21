using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.IRepository;
using App.IServices;
using App.Common.Utils;
using App.Common.Auth;
using App.Entities.Dtos;

namespace App.Services
{
    public class QQUserinfoLogic : BaseLogic<QQUserinfo>, IQQUserinfoLogic
    {
        IQQUserinfoRepository _qQUserinfoRepository;
        public QQUserinfoLogic(IQQUserinfoRepository qQUserinfoRepository) : base(qQUserinfoRepository)
        {
            _qQUserinfoRepository = qQUserinfoRepository;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="code">回调返回的code</param>
        /// <param name="state">自定义验证状态码</param>
        /// <returns></returns>
        public QQUserinfo Login(string code, string state)
        {
            //token需要缓存起来，有效期30天
            var token = QQAuthorize.GetAccessToken(code, state);
            string openid = QQAuthorize.GetOpenId(token.access_token);
            QQUserinfo userinfo = _qQUserinfoRepository.FindEntity(c => c.OpenId == openid);
            if (userinfo == null)
            {
                var user = QQAuthorize.GetUserInfo(token.access_token, openid);
                userinfo = new QQUserinfo()
                {
                    UserId = SnowflakeUtil.NextStringId(),
                    NikeName = user.nickname,
                    Gender = user.gender,
                    Birthday = user.year,
                    OpenId = openid,
                    Province = user.province,
                    Image40 = user.figureurl_qq_1,
                    Image100 = user.figureurl_qq_2,
                    LastLoginTime = DateTime.Now,
                    AccessToken = token.access_token,
                    RefreshToken = token.refresh_token,
                    ExpireDate = DateTime.Now.AddSeconds(token.expires_in)
                };
                _qQUserinfoRepository.Insert(userinfo);
            }
            else
            {
                userinfo.LastLoginTime = DateTime.Now;
                userinfo.AccessToken = token.access_token;
                userinfo.RefreshToken = token.refresh_token;
                userinfo.ExpireDate = DateTime.Now.AddSeconds(token.expires_in);
                _qQUserinfoRepository.Update(userinfo, i => new { i.CreatorTime, i.IsMaster, i.OpenId });
            }
            return userinfo;
        }

        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="url">登录成功后返回的地址</param>
        /// <returns>授权地址</returns>
        public OperateResult<string> Authorize(string curl)
        {
            string url = QQAuthorize.AuthorizeLoginUrl(curl);
            return new OperateResult<string>() { Data = url, Status = ResultStatus.Success };
        }
    }
}
