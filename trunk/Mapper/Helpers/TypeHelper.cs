using System;

namespace Mapper.Helpers
{
    internal static class TypeHelper
    {
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        public static Type GetCollectionElementType(Type collectionType)
        {
            if (collectionType.IsArray)
            {
                return collectionType.GetElementType();
            }
            return collectionType.GetGenericArguments()[0];
        }

        public static Type GetDictionaryValueType(Type type)
        {
            return type.GetGenericArguments()[1];
        }

        public static Type GetDictionaryKeyType(Type type)
        {
            return type.GetGenericArguments()[0];
        }
    }
}