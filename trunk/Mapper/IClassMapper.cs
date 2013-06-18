using System;

namespace Mapper
{
    public interface IClassMapper
    {
        DynamicVariantType Store(object memento);
        object Restore(Type type, DynamicVariantType storage);
    }
}