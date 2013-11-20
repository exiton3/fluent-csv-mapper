using Mapper.Converters;

namespace Mapper.Configuration
{
    public interface IConverterPropertyMapOptions
    {
        void UseTypeConverter<TConverter>() where TConverter : ITypeConverter, new();
    }
}