using System;
using Mapper.Configuration;
using Mapper.Converters;

namespace Mapper.Tests.ConcreteClasses
{
    class DateToStringFormatter: ValueFormatter<DateTime>
    {
        protected override string Format(DateTime source)
        {
            return source.ToShortDateString();
        }

        protected override DateTime Parse(string str)
        {
            return DateTime.Parse(str);
        }
    }
}