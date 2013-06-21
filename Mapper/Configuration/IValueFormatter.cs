namespace Mapper.Configuration
{
    public interface IValueFormatter
    {
        string Format(object value);
        object Parse(string str);
    }
}