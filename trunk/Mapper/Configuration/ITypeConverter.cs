namespace Mapper.Configuration
{
    public interface ITypeConverter
    {
        object Convert(object value);
        object ConvertBack(object value);
    }
}