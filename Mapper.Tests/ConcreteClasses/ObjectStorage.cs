using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mapper.Tests.ConcreteClasses
{
    public class ObjectStorage : ISerializable, IEnumerable, IObjectStorage
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public IEnumerable<KeyValuePair<string, object>> Data { get { return _data; } }

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

        public object GetData(string key)
        {
            object value;
            _data.TryGetValue(key, out value);
            return value;
        }
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            
            foreach (var keyValuePair in Data)
            {

                if (keyValuePair.Value is IList)
                {
                    stringBuilder.AppendFormat("[ {0}, ", keyValuePair.Key);
                    var collection = (IList) keyValuePair.Value;

                    foreach (var item in collection)
                    {
                        stringBuilder.Append(item);
                    }
                    stringBuilder.AppendLine(" ]");
                }
                else
                {
                    stringBuilder
                                 .AppendFormat("[ {0}, {{{1}}}", keyValuePair.Key, keyValuePair.Value)
                                 .AppendLine(" ]");
                }
            }

            return stringBuilder.ToString();
        }
    }
}