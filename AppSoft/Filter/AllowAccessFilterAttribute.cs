using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppSoft.Filter
{
    /// <summary>
    /// 标记此特性不需要访问权限验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAccessFilterAttribute : Attribute
    {

    }
}
