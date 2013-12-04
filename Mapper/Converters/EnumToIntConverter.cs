using System;

namespace Mapper.Converters
{
    public class EnumToIntConverter<TEnum> : TypeConverter<TEnum, int> where TEnum:IConvertible
    {
        protected override int Convert(TEnum source)
        {

            if (!typeof (TEnum).IsEnum)
                throw new ArgumentException("Generic Type must be a System.Enum");
            return System.Convert.ToInt32(source);
        }

        protected override TEnum ConvertBack(int source)
        {
            if (!Enum.IsDefined(typeof (TEnum), source))
            {
                throw new InvalidOperationException(
                    String.Format("Can not convert int value {0} to Enum {1}. The value not defined in enum", source,
                                  typeof (TEnum)));
            }
            return (TEnum) Enum.ToObject(typeof (TEnum), source);
        }
    }
}