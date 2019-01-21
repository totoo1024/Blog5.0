using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.IServices;
using App.IRepository;
using App.Entities.Dtos;
using App.Common.Utils;
using App.Common.Net;

namespace App.Services
{
    public class LeavemsgLogic : BaseLogic<LeavemsgInfo>, ILeavemsgLogic
    {
        ILeavemsgRepository _leavemsgRepository;
        IQQUserinfoLogic _qQUserinfoLogic;
        IArticleInfoLogic _articleInfoLogic;
        public LeavemsgLogic(ILeavemsgRepository leavemsgRepository, IQQUserinfoLogic qQUserinfoLogic, IArticleInfoLogic articleInfoLogic) : base(leavemsgRepository)
        {
            _leavemsgRepository = leavemsgRepository;
            _qQUserinfoLogic = qQUserinfoLogic;
            _articleInfoLogic = articleInfoLogic;
        }

        /// <summary>
        /// 评论留言
        /// </summary>
        /// <param name="index">当前页面</param>
        /// <param name="pagesize">每页显示的条数</param>
        /// <param name="childsize">评论下显示的回复数量</param>
        /// <param name="aid">文章id</param>
        /// <returns></returns>
        public PageResult<List<CommentDto>> MsgList(int index, int pagesize, int childsize, string aid = null)
        {
            var tuple = _leavemsgRepository.MsgList(index, pagesize, childsize, aid);
            PageResult<List<CommentDto>> result = new PageResult<List<CommentDto>>();
            result.code = 0;
            result.data = tuple.Item1;
            double no = tuple.Item2 * 1d / pagesize;
            result.count = (int)Math.Ceiling(no);
            return result;
        }

        /// <summary>
        /// 回复分页
        /// </summary>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">没有显示条数</param>
        /// <param name="rootid">评论ID</param>
        /// <param name="aid">文章ID</param>
        /// <returns></returns>
        public PageResult<List<ReplyDto>> ReplyList(int pageindex, int pagesize, string rootid, string aid)
        {
            PageResult<List<ReplyDto>> result = new PageResult<List<ReplyDto>>();
            var tuple = _leavemsgRepository.ReplyList(pageindex, pagesize, rootid, aid);
            result.code = 0;
            result.data = tuple.Item1;
            result.count = tuple.Item2;
            return result;
        }

        /// <summary>
        /// 留言评论
        /// </summary>
        /// <param name="content">留言内容</param>
        /// <param name="userid">评论人id</param>
        /// <param name="articleId">文章id</param>
        /// <returns></returns>
        public OperateResult Comment(string content, string userid, string articleId)
        {
            OperateResult result = new OperateResult();
            if (string.IsNullOrWhiteSpace(content))
            {
                result.Message = "请输入留言内容";
                return result;
            }
            if (content.Length > 500)
            {
                result.Message = "回复内容超出长度";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(articleId))
                {
                    if (_articleInfoLogic.QueryableCount(c => c.ArticleId == articleId) == 0)
                    {
                        result.Message = "评论的文章不存在";
                        return result;
                    }
                }
                LeavemsgInfo leavemsg = new LeavemsgInfo();
                leavemsg.FromUId = userid;
                leavemsg.Content = content;
                leavemsg.IP = HttpHelper.GetClientIp();
                leavemsg.Adscription = HttpHelper.GetAddressByApi();
                leavemsg.MsgId = SnowflakeUtil.NextStringId();
                leavemsg.ParentId = leavemsg.MsgId;
                leavemsg.ArticleId = articleId;
                Insert(leavemsg);
                result.Status = ResultStatus.Success;
            }
            return result;
        }

        /// <summary>
        /// 回复留言/评论
        /// </summary>
        /// <param name="rootid">评论ID</param>
        /// <param name="pid">上级评论id</param>
        /// <param name="fromid">回复人id</param>
        /// <param name="content">回复内容</param>
        /// <param name="userid">评论回复人id</param>
        /// <param name="articleId">文章id</param>
        /// <returns></returns>
        public OperateResult Reply(string rootid, string pid, string fromid, string content, string userid, string articleId)
        {
            OperateResult result = new OperateResult();
            if (string.IsNullOrWhiteSpace(content))
            {
                result.Message = "请输入留言内容";
                return result;
            }
            if (content.Length > 500)
            {
                result.Message = "回复内容超出长度";
                return result;
            }
            if (fromid == userid)
            {
                result.Message = "不能回复自己的留言";
                return result;
            }
            if (string.IsNullOrWhiteSpace(rootid) || string.IsNullOrWhiteSpace(fromid))
            {
                result.Message = "缺少参数";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(pid) || rootid == pid)
                {
                    if (_leavemsgRepository.QueryableCount(c => c.MsgId == rootid) == 0)
                    {
                        result.Message = "请选择回复内容";
                        return result;
                    }
                }
                else
                {
                    if (_leavemsgRepository.QueryableCount(c => c.MsgId == rootid || c.MsgId == pid) != 2)
                    {
                        result.Message = "请选择回复内容";
                        return result;
                    }
                }
                if (!string.IsNullOrWhiteSpace(articleId))
                {
                    if (_articleInfoLogic.QueryableCount(c => c.ArticleId == articleId) == 0)
                    {
                        result.Message = "评论的文章不存在";
                        return result;
                    }
                }
                LeavemsgInfo leavemsg = new LeavemsgInfo();
                leavemsg.FromUId = userid;
                leavemsg.ToUId = fromid;
                leavemsg.RootId = rootid;
                leavemsg.ParentId = pid ?? rootid;
                leavemsg.Content = content;
                leavemsg.IP = HttpHelper.GetClientIp();
                leavemsg.Adscription = HttpHelper.GetAddressByApi();
                leavemsg.MsgId = SnowflakeUtil.NextStringId();
                leavemsg.ArticleId = articleId;
                Insert(leavemsg);
                result.Status = ResultStatus.Success;
            }
            return result;
        }

    }
}
