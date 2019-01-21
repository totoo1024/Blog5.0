using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.IRepository;
using App.IServices;

namespace App.Services
{
    public class ArticleTagsLogic : BaseLogic<ArticleTags>, IArticleTagsLogic
    {
        IArticleTagsRepository _articleTagsRepository;
        public ArticleTagsLogic(IArticleTagsRepository articleTagsRepository) : base(articleTagsRepository)
        {
            _articleTagsRepository = articleTagsRepository;
        }
    }
}
