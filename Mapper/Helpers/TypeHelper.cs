using System;

namespace Mapper.Helpers
{
    public static class TypeHelper
    {
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }
    }
}