using Mapper.Configuration;

namespace Mapper.Mappers
{
    internal interface IObjectMappingStrategyFactory
    {
        IObjectMappingStrategy Create(IPropertyMapInfo propertyMapInfo, IClassMapper classMapper);
    }

    internal class ObjectMappingStrategyFactory : IObjectMappingStrategyFactory
    {
        private InheritanceObjectMappingStrategy _inheritanceObjectMappingStrategy;
        private SimpleObjectMappingStrategy _simpleObjectMappingStrategy;

        public IObjectMappingStrategy Create(IPropertyMapInfo propertyMapInfo, IClassMapper classMapper)
        {
            if (propertyMapInfo.IsDiscriminatorSet)
            {
                return _inheritanceObjectMappingStrategy ??
                       (_inheritanceObjectMappingStrategy = new InheritanceObjectMappingStrategy(classMapper));
            }
            return _simpleObjectMappingStrategy ??
                   (_simpleObjectMappingStrategy = new SimpleObjectMappingStrategy(classMapper));
        }
    }
}