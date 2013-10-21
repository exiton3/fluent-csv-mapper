using System;

namespace Mapper.Converters
{
    public interface ITypeConverter
    {
        object Convert(object value);
        object ConvertBack(object value);
        Type SourceType { get; }
        Type DestinationType { get; }
    }
}