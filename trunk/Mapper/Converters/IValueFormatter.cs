namespace Mapper.Converters
{
    public interface IValueFormatter
    {
        string Format(object value);
        object Parse(string str);
    }
}