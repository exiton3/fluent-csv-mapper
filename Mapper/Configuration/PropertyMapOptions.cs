using Mapper.Converters;

namespace Mapper.Configuration
{
    internal class PropertyMapOptions<T> : IPropertyMapOptions, IReferencePropertyMapOptions,IDescriminateMapOptions
    {
        private readonly PropertyMapInfo<T> _propertyMapInfo;

        public PropertyMapOptions(PropertyMapInfo<T> propertyMapInfo)
        {
            _propertyMapInfo = propertyMapInfo;
        }

        public IDescriminateMapOptions DiscriminateOnField(string name)
        {
            _propertyMapInfo.DiscriminatorField = name;

            return this;
        }

        public IDescriminateMapOptions DiscriminatorValueFor<TValue>(string value) where TValue : class
        {
            _propertyMapInfo.DiscriminatorTypes.Add(value, typeof (TValue));
            return this;
        }

        public void UseFormatter<TFormatter>() where TFormatter : IValueFormatter, new()
        {
            _propertyMapInfo.ValueFormatter = new TFormatter();
        }

        public void UseTypeConverter<TConverter>() where TConverter : ITypeConverter, new()
        {
            _propertyMapInfo.TypeConverter = new TConverter();
        }
    }
}