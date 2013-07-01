using System.Collections.Generic;

namespace Mapper
{
    public interface IObjectStorage
    {
        IEnumerable<KeyValuePair<string, object>> Data { get; }
        void SetData(string key, object objectStorage);
        object GetData(string key);
    }
}