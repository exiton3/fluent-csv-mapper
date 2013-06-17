using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Mapper
{
    public class DynamicVariantType : ISerializable, IEnumerable
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public Dictionary<string,object> Data { get { return _data; } }

        public DynamicVariantType() { }

        protected DynamicVariantType(SerializationInfo info, StreamingContext context)
        {
            _data = (Dictionary<string, object>)info.GetValue("_data", typeof(Dictionary<string, object>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_data", _data);
        }

        public DynamicVariantType GetVariant(string property)
        {
            return (DynamicVariantType)this[property];
        }

        public DynamicVariantType[] GetVariantArray(string property)
        {
            var result = this[property];

            // This is needed for backward compatibility with dynamics persistence
            if (result is object[])
                return ((object[])result).Cast<DynamicVariantType>().ToArray();

            return (DynamicVariantType[])result;
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

        public void Add(string index, object item)
        {
            _data.Add(index, item);
        }

        public IEnumerator GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}