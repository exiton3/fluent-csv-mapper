namespace Mapper.Configuration
{
    public interface IPropertyMapOptions : IFluentSyntax
    {
        void UseFormatter<TFormatter>() where TFormatter : IValueFormatter, new();
    }
}