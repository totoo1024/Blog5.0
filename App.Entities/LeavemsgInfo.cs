using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace App.Entities
{
    /// <summary>
    /// 留言信息
    /// </summary>
    public class LeavemsgInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string MsgId { get; set; }

        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleId { get; set; }

        /// <summary>
        /// 顶级评论ID
        /// </summary>
        public string RootId { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 评论人ID（对应QQ用户ID）
        /// </summary>
        public string FromUId { get; set; }

        /// <summary>
        /// 回复人ID（对应QQ用户ID）
        /// </summary>
        public string ToUId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 归属地
        /// </summary>
        public string Adscription { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool DeleteMark { get; set; } = false;

        /// <summary>
        /// 创建日期
        /// </summary>
        public System.DateTime CreatorTime { get; set; } = DateTime.Now;
    }
}
