using Mapper.Configuration;

namespace Mapper.Mappers
{
    public interface IMapper
    {
        object Store(IPropertyMapInfo propertyMapInfo, object objectToStore, IClassMapper classMapper);
        object Restore(IPropertyMapInfo propertyMapInfo, object value,IClassMapper classMapper);
        bool IsMatch(IPropertyMapInfo propertyMapInfo);

    }
}