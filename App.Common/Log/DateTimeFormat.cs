using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Log
{
    public class DateTimeFormat : IsoDateTimeConverter
    {
        public DateTimeFormat()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}
