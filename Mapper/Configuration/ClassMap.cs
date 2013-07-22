using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Mapper.Helpers;

namespace Mapper.Configuration
{
    public abstract class ClassMap<T> : IClassMap where T : class,new()
    {
        private readonly Dictionary<string, IPropertyMapInfo> _mappings = new Dictionary<string,IPropertyMapInfo>();

        private PropertyMapOptions<T> _propertyMapOptions;

        public object Instance { get { return new T(); } }

        public Dictionary<string, IPropertyMapInfo> Mappings
        {
            get { return _mappings; }
        }

        public bool IsMappingForPropertyExist(string name)
        {
            return _mappings.ContainsKey(name);
        }

        public IPropertyMapInfo GetMapping(string name)
        {
            IPropertyMapInfo mapping;
            if (_mappings.TryGetValue(name, out mapping))
            {
                return _mappings[name];
            }
            throw new MapperMappingException(string.Format("Mapping for property {0} was not found in {1} mapping class.", name, GetType().Name), name);
        }


        protected IPropertyMapOptions Map<TValue>(Expression<Func<T, TValue>> getterExpression,string name)
        {
            var propInfo = CreatePropertyMapInfo(getterExpression);
            _propertyMapOptions = new PropertyMapOptions<T>(propInfo);
            _mappings.Add(name, propInfo);
            return _propertyMapOptions;
        }

        protected void MapAsReference<TValue>(Expression<Func<T, TValue>> getterExpression, string name)
        {
            var propInfo = CreatePropertyMapInfo(getterExpression);
            propInfo.ReferenceType = typeof (TValue);
            _mappings.Add(name, propInfo);
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