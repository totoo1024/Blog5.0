using System;
using System.Collections.Generic;
using System.Text;
using App.IRepository;
using App.Core;
using App.Entities;

namespace App.Repository
{
    public class ArticleCategoryRepository : BaseRepository<ArticleCategory>, IArticleCategoryRepository
    {
    }
}
