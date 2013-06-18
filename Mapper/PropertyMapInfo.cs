using System;

namespace Mapper
{
    interface IPropertyMapInfo<T> where T:class 
    {
        Func<T, object> Getter { get; set; }
        Action<T, object> Setter { get; set; }
    }

    public sealed class PropertyMapInfo<T> : IPropertyMapInfo<T>, IPropertyMapInfo where T:class
    {
        public Func<T, object> Getter { get; set; }

        public Action<T, object> Setter { get; set; }

        Action<object, object> IPropertyMapInfo.Setter
        {
            get { return (o, p) => Setter((T) o, p); }
        }

        Func<object, object> IPropertyMapInfo.Getter
        {
            get {return o => Getter((T)o); }
        }

        public IValueFormatter ValueFormatter { get; set; }

        public bool IsValueFormatterSetted
        {
            get { return ValueFormatter != null; }
        }

        public bool IsReferenceProperty
        {
            get {return ReferenceType != null; }
        }

        public Type ClassMapping { get; set; }

        public Type ReferenceType {get; set; }
    }
    
}