using System;
using Mapper.Converters;

namespace Mapper.Tests.ConcreteClasses
{
    internal class DateTimeFormatter : ValueFormatter<DateTime>
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