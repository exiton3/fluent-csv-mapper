using System;
using Mapper.Configuration;

namespace Mapper.Tests.ConcreteClasses
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