using System;

namespace Mapper
{
    public interface IClassMapper
    {
        IObjectStorage Store(object memento);
        object Restore(Type type, IObjectStorage storage);
        bool CanMap(Type type);
    }
}