using System;

namespace Mapper
{
    public interface IClassMapper
    {
        IObjectStorage Store(object objectToStore);
        object Restore(Type type, IObjectStorage storage);
        bool CanMap(Type type);
    }
}