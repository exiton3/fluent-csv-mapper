using System;

namespace Mapper
{
    public interface IClassMapper
    {
        ObjectStorage Store(object memento);
        object Restore(Type type, ObjectStorage storage);
    }
}