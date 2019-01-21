using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.Core;
using App.Entities.Dtos;

namespace App.IRepository
{
    public interface ILeavemsgRepository : IBaseRepository<LeavemsgInfo>
    {
        /// <summary>
        /// 留言评论
        /// </summary>
        /// <param name="index">当前页</param>
        /// <param name="pagesize">每页显示的行数</param>
        /// <param name="childsize">评论下显示的回复数量</param>
        /// <param name="aid">文章id</param>
        /// <returns></returns>
        Tuple<List<CommentDto>, int> MsgList(int index, int pagesize, int childsize, string aid = null);

        /// <summary>
        /// 回复分页
        /// </summary>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">没有显示条数</param>
        /// <param name="rootid">评论ID</param>
        /// <param name="aid">文章ID</param>
        /// <returns></returns>
        Tuple<List<ReplyDto>, int> ReplyList(int pageindex, int pagesize, string rootid, string aid);
    }
}
