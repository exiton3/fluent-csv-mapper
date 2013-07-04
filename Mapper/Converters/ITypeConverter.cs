namespace Mapper.Converters
{
    public interface ITypeConverter
    {
        object Convert(object value);
        object ConvertBack(object value);
    }
}