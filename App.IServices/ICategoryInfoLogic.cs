using System;
using System.Collections.Generic;
using System.Text;
using App.Entities;
using App.Entities.Dtos;

namespace App.IServices
{
    public interface ICategoryInfoLogic : IBaseLogic<CategoryInfo>
    {
        /// <summary>
        /// 新增修改栏目
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        OperateResult Save(CategoryInfo category);
    }
}
