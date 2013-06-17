namespace Mapper
{
    public interface IPropertyMapOptions : IFluentSyntax
    {
        void UseFormatter<TFormatter>() where TFormatter : IValueFormatter, new();
        void UseMapping<TClassMap>() where TClassMap : IMapConfiguration;
    }
}