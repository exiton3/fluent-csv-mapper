using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mapper
{
    public class ClassMap<T> : IMapConfiguration where T : class,new()
    {
        private readonly Dictionary<string, IPropertyMapInfo> _mappings = new Dictionary<string,IPropertyMapInfo>();

        private PropertyMapOptions<T> _propertyMapOptions;

        public object Instance { get { return new T(); } }

        public Dictionary<string, IPropertyMapInfo> Mappings
        {
            get { return _mappings; }
        }

        public IPropertyMapInfo GetMapping(string name)
        {
            return _mappings[name];
        }


        protected IPropertyMapOptions Map<TValue>(Expression<Func<T, TValue>> getterExpression,string name)
        {
            var propInfo = CreatePropertyMapInfo(getterExpression);
            _propertyMapOptions = new PropertyMapOptions<T>(propInfo);
            _mappings.Add(name, propInfo);
            return _propertyMapOptions;
        }

       
        private PropertyMapInfo<T> CreatePropertyMapInfo<TValue>(Expression<Func<T, TValue>> getterExpression)
        {
            var setter = new Action<T, object>((o, v) => PropertyExpressionHelper.InitializeSetter(getterExpression)(o, (TValue) v));
            var getter = new Func<T, object>(o => PropertyExpressionHelper.InitializeGetter(getterExpression)(o));

            var propInfo = new PropertyMapInfo<T>
                               {
                                   Setter = setter,
                                   Getter = getter
                               };
            return propInfo;
        }

    }
}