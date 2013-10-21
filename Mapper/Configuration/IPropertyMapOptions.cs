using Mapper.Converters;
using Mapper.Helpers;

namespace Mapper.Configuration
{
    public interface IPropertyMapOptions : IFluentSyntax, IReferencePropertyMapOptions
    {
        void UseFormatter<TFormatter>() where TFormatter : IValueFormatter, new();
    }
}