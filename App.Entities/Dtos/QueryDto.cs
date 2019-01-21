using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Dtos
{
    /// <summary>
    /// 分页查询实体
    /// </summary>
    public class QueryDto
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 每页显示的条数
        /// </summary>
        public int limit { get; set; }

        /// <summary>
        /// 排序字段加asc/desc
        /// </summary>
        public string order { get; set; }

        private List<Condition> _conditions;

        /// <summary>
        /// 条件（接收客户端输入的条件参数）
        /// </summary>
        public List<Condition> conditions
        {
            get { return _conditions; }
            set
            {
                _conditions = value;
                if (value.Any())
                {
                    //List<IConditionalModel> list = new List<IConditionalModel>();
                    foreach (var item in conditions)
                    {
                        //List<IConditionalModel> models = new List<IConditionalModel>();
                        ConditionalModel conditional = new ConditionalModel()
                        {
                            FieldName = item.name,
                            FieldValue = item.value,
                            ConditionalType = GetOperator(item.op)
                        };
                        ConditionalModels.Add(conditional);
                    }
                    //ConditionalModels = list;
                }
            }
        }

        /// <summary>
        /// 条件（供底层条件查询）
        /// </summary>
        public List<IConditionalModel> ConditionalModels { get; private set; } = new List<IConditionalModel>();

        /// <summary>
        /// 获取操作符
        /// </summary>
        /// <param name="op">操作符简写</param>
        /// <returns></returns>
        private ConditionalType GetOperator(string op)
        {
            ConditionalType type = ConditionalType.Equal;
            switch (op.ToLower())
            {
                case "eq":
                    type = ConditionalType.Equal;
                    break;
                case "gt"://大于
                    type = ConditionalType.GreaterThan;
                    break;
                case "ge"://大于或等于
                    type = ConditionalType.GreaterThanOrEqual;
                    break;
                case "in"://包括
                    type = ConditionalType.In;
                    break;
                case "lt"://小于
                    type = ConditionalType.LessThan;
                    break;
                case "le"://小于或等于
                    type = ConditionalType.LessThanOrEqual;
                    break;
                case "cn"://包含
                    type = ConditionalType.Like;
                    break;
                case "sw"://以...开始
                    type = ConditionalType.LikeLeft;
                    break;
                case "ew"://以什么结束
                    type = ConditionalType.LikeRight;
                    break;
                case "ne"://不等于
                    type = ConditionalType.NoEqual;
                    break;
                case "nc"://不包含
                    type = ConditionalType.NoLike;
                    break;
                case "ni"://不包括
                    type = ConditionalType.NotIn;
                    break;
            }
            return type;
        }

    }

    public class Condition
    {
        /// <summary>
        /// 查询字段名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 查询比较符号
        /// </summary>
        public string op { get; set; }

    }
}
