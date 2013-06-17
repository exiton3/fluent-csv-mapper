using System;

namespace Mapper
{
    public interface IClassMapper
    {
        //DynamicVariantType Store<T>(T memento) where T : class;
        //T Restore<T>(DynamicVariantType storage) where T : class,new();

        DynamicVariantType Store(object memento);
        object Restore(Type type, DynamicVariantType storage);
    }
}