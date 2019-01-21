using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppSoft.Models
{
    public class TimeLineDto
    {
        public int Year { get; set; }

        public Dictionary<string, IEnumerable<LineItem>> Items { get; set; }
    }

    public class LineItem
    {
        public string Time { get; set; }

        public string Content { get; set; }
    }
}
