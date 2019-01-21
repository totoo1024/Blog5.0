using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.IRepository;
using App.IServices;
using App.Entities.Dtos;
using App.Common.Utils;

namespace App.Services
{
    public class TagsInfoLogic : BaseLogic<TagsInfo>, ITagsInfoLogic
    {
        ITagsInfoRepository _tagsInfoRepository;
        public TagsInfoLogic(ITagsInfoRepository tagsInfoRepository) : base(tagsInfoRepository)
        {
            _tagsInfoRepository = tagsInfoRepository;
        }

        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="tags">标签信息</param>
        /// <returns></returns>
        public OperateResult Save(TagsInfo tags)
        {
            if (QueryableCount(c => c.TagName == tags.TagName && c.TagId != tags.TagId && tags.DeleteMark == false) > 0)
            {
                return new OperateResult("标签已存在，请勿重复添加");
            }
            if (string.IsNullOrWhiteSpace(tags.TagId))
            {
                tags.TagId = SnowflakeUtil.NextStringId();
                return Insert(tags);
            }
            else
            {
                return Update(tags, i => new { i.CreatorTime, i.EnabledMark });
            }
        }

        /// <summary>
        /// 查询各个标签文章数量
        /// </summary>
        /// <returns></returns>
        public dynamic TagsCount()
        {
            return _tagsInfoRepository.TagsCount();
        }
    }
}
