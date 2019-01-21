using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.Entities.Dtos;

namespace App.IServices
{
    public interface ILeavemsgLogic : IBaseLogic<LeavemsgInfo>
    {
        /// <summary>
        /// 评论留言
        /// </summary>
        /// <param name="index">当前页面</param>
        /// <param name="pagesize">每页显示的条数</param>
        /// <param name="childsize">评论下显示的回复数量</param>
        /// <param name="aid">文章id</param>
        /// <returns></returns>
        PageResult<List<CommentDto>> MsgList(int index, int pagesize, int childsize, string aid = null);

        /// <summary>
        /// 留言评论
        /// </summary>
        /// <param name="content">留言内容</param>
        /// <param name="userid">评论人id</param>
        /// <param name="articleId">文章id</param>
        /// <returns></returns>
        OperateResult Comment(string content, string userid, string articleId);

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
        OperateResult Reply(string rootid, string pid, string fromid, string content, string userid, string articleId);

        /// <summary>
        /// 回复分页
        /// </summary>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">没有显示条数</param>
        /// <param name="rootid">评论ID</param>
        /// <param name="aid">文章ID</param>
        /// <returns></returns>
        PageResult<List<ReplyDto>> ReplyList(int pageindex, int pagesize, string rootid, string aid);
    }
}
