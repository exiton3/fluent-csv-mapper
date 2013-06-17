using System;

namespace Mapper.Tests
{
    class DateTimeFormatter:IValueFormatter
    {
        public string Format(object value)
        {
            return ((DateTime) value).ToShortDateString();
        }

        public object Parse(string str)
        {
            return DateTime.Parse(str);
        }
    }
}