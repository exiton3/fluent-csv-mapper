namespace Mapper
{
    public interface IValueFormatter
    {
        string Format(object value);
        object Parse(string str);
    }
}