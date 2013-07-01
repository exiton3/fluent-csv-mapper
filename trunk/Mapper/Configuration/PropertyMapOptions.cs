namespace Mapper.Configuration
{
    internal class PropertyMapOptions<T> : IPropertyMapOptions where T : class
    {
        private readonly PropertyMapInfo<T> _propertyMapInfo;

        public PropertyMapOptions(PropertyMapInfo<T> propertyMapInfo)
        {
            _propertyMapInfo = propertyMapInfo;
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