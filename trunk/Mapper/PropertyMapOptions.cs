namespace Mapper
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
    }

}