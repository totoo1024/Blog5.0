using System;
using System.Collections.Generic;
using System.Text;
using App.Core;
using App.Entities;
using App.Entities.Dtos;
using App.IRepository;
using SqlSugar;

namespace App.Repository
{
    public class LeavemsgRepository : BaseRepository<LeavemsgInfo>, ILeavemsgRepository
    {
        /// <summary>
        /// 留言评论
        /// </summary>
        /// <param name="index">当前页</param>
        /// <param name="pagesize">每页显示的行数</param>
        /// <param name="childsize">评论下显示的回复数量</param>
        /// <param name="aid">文章id</param>
        /// <returns></returns>
        public Tuple<List<CommentDto>, int> MsgList(int index, int pagesize, int childsize, string aid = null)
        {
            int total = 0;
            List<CommentDto> comments = Db.Queryable<LeavemsgInfo, QQUserinfo>((msg, user) => msg.FromUId == user.UserId)
                .Where(msg => msg.DeleteMark == false && SqlFunc.IsNullOrEmpty(msg.RootId))
                .WhereIF(SqlFunc.IsNullOrEmpty(aid), msg => SqlFunc.IsNullOrEmpty(msg.ArticleId))
                .WhereIF(!SqlFunc.IsNullOrEmpty(aid), msg => msg.ArticleId == aid)
                .OrderBy(msg => msg.CreatorTime, OrderByType.Desc)
                .Select<CommentDto>().Mapper((it, cache) =>
                {
                    int count = 0;
                    it.Reply = Db.Queryable<LeavemsgInfo, QQUserinfo, QQUserinfo>((m, u1, u2) =>
                        new object[] { JoinType.Left, m.FromUId == u1.UserId, JoinType.Left, m.ToUId == u2.UserId }
                     ).Where(m => m.RootId == it.MsgId)
                     .OrderBy(m => m.ParentId, OrderByType.Asc)
                     .OrderBy(m => m.MsgId, OrderByType.Asc)
                     .OrderBy(m => m.CreatorTime, OrderByType.Asc)
                     .Select((m, u1, u2) => new ReplyDto()
                     {
                         MsgId = m.MsgId,
                         FromUId = m.FromUId,
                         FromNikeName = u1.NikeName,
                         FromImage = u1.Image40,
                         ToUId = m.ToUId,
                         ToNikeName = u2.NikeName,
                         ToImage = u2.Image40,
                         Content = m.Content,
                         Adscription = m.Adscription,
                         IsMaster = u1.IsMaster,
                         CreatorTime = m.CreatorTime
                     })
                     .ToPageList(1, childsize, ref count);
                    it.page = (int)Math.Ceiling(count * 1d / childsize);
                })
                .ToPageList(index, pagesize, ref total);
            return Tuple.Create(comments, total);
        }

        /// <summary>
        /// 回复分页
        /// </summary>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">没有显示条数</param>
        /// <param name="rootid">评论ID</param>
        /// <param name="aid">文章ID</param>
        /// <returns></returns>
        public Tuple<List<ReplyDto>, int> ReplyList(int pageindex, int pagesize, string rootid, string aid)
        {
            int count = 0;
            List<ReplyDto> reply = Db.Queryable<LeavemsgInfo, QQUserinfo, QQUserinfo>((m, u1, u2) =>
               new object[] { JoinType.Left, m.FromUId == u1.UserId, JoinType.Left, m.ToUId == u2.UserId }
             ).Where(m => m.RootId == rootid && m.DeleteMark == false)
             .WhereIF(SqlFunc.IsNullOrEmpty(aid), m => SqlFunc.IsNullOrEmpty(m.ArticleId))
             .WhereIF(!SqlFunc.IsNullOrEmpty(aid), m => m.ArticleId == aid)
             .OrderBy(m => m.ParentId, OrderByType.Asc)
             .OrderBy(m => m.MsgId, OrderByType.Asc)
             .OrderBy(m => m.CreatorTime, OrderByType.Asc)
             .Select((m, u1, u2) => new ReplyDto()
             {
                 MsgId = m.MsgId,
                 FromUId = m.FromUId,
                 FromNikeName = u1.NikeName,
                 FromImage = u1.Image40,
                 ToUId = m.ToUId,
                 ToNikeName = u2.NikeName,
                 ToImage = u2.Image40,
                 Content = m.Content,
                 Adscription = m.Adscription,
                 IsMaster = u1.IsMaster,
                 CreatorTime = m.CreatorTime
             })
             .ToPageList(pageindex, pagesize, ref count);
            int page = (int)Math.Ceiling(count * 1d / pagesize);
            return Tuple.Create(reply, page);
        }
    }
}
