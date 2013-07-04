namespace Mapper.Converters
{
    public abstract class ValueFormatter<TSource> : IValueFormatter
    {
        public string Format(object value)
        {
            return Format((TSource) value);
        }

        object IValueFormatter.Parse(string str)
        {
            return Parse(str);
        }

        protected abstract string Format(TSource source);

        protected abstract TSource Parse(string str);
    }
}