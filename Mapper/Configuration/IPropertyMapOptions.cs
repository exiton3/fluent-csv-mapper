using Mapper.Converters;
using Mapper.Helpers;

namespace Mapper.Configuration
{
    public interface IPropertyMapOptions : IFluentSyntax
    {
        void UseFormatter<TFormatter>() where TFormatter : IValueFormatter, new();
        void UseTypeConverter<TConverter>() where TConverter : ITypeConverter, new();
    }
}