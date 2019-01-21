using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Dtos
{
    /// <summary>
    /// 树形类
    /// </summary>
    public class TreeModuleDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 父级id
        /// </summary>
        public string pid { get; set; }
        /// <summary>
        /// 是否展开
        /// </summary>
        public bool open { get; set; } = true;
    }
}
