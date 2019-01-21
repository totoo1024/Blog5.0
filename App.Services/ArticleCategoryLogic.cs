using System;
using System.Collections.Generic;
using System.Text;
using App.IServices;
using App.Entities;
using App.IRepository;

namespace App.Services
{
    public class ArticleCategoryLogic : BaseLogic<ArticleCategory>, IArticleCategoryLogic
    {
        IArticleCategoryRepository _articleCategoryRepository;
        public ArticleCategoryLogic(IArticleCategoryRepository articleCategoryRepository) : base(articleCategoryRepository)
        {
            _articleCategoryRepository = articleCategoryRepository;
        }
    }
}
