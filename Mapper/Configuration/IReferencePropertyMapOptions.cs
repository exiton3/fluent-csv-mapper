using Mapper.Converters;

namespace Mapper.Configuration
{
    public interface IReferencePropertyMapOptions
    {
        void UseTypeConverter<TConverter>() where TConverter : ITypeConverter, new();
    }
}