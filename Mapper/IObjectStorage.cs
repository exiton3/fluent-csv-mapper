using System.Collections.Generic;

namespace Mapper
{
    public interface IObjectStorage
    {
        Dictionary<string, object> Data { get; }
        void SetData(string key, object objectStorage);
    }
}