using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mapper
{
    public class ObjectStorage : ISerializable, IEnumerable
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public Dictionary<string, object> Data { get { return _data; } }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_data", _data);
        }

        public object this[string index]
        {
            get
            {
                object value;
                _data.TryGetValue(index, out value);
                return value;
            }
            set { _data[index] = value; }
        }

        public IEnumerator GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public virtual void SetData(string key, object objectStorage)
        {
            _data[key] = objectStorage;
        }
    }
}