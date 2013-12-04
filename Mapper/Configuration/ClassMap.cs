using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Mapper.Helpers;

namespace Mapper.Configuration
{
    public abstract class ClassMap<T> : IClassMap where T : new()
    {
        private readonly Dictionary<string, IPropertyMapInfo> _mappings = new Dictionary<string, IPropertyMapInfo>();

        private PropertyMapOptions<T> _propertyMapOptions;

        public object Instance
        {
            get { return new T(); }
        }

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
            throw new MapperMappingException(
                string.Format("Mapping for property {0} was not found in {1} mapping class.", name, GetType().Name),
                name);
        }


        protected IPropertyMapOptions Map<TValue>(Expression<Func<T, TValue>> getterExpression, string name)
        {
            var propInfo = CreatePropertyMapInfo(getterExpression, PropertyKind.Value);
            propInfo.PropertyType = typeof (TValue);
            _propertyMapOptions = new PropertyMapOptions<T>(propInfo);
            _mappings.Add(name, propInfo);
            return _propertyMapOptions;
        }

        protected IReferencePropertyMapOptions MapAsReference<TValue>(Expression<Func<T, TValue>> getterExpression, string name)
        {
            var propInfo = CreatePropertyMapInfo(getterExpression, PropertyKind.Reference);
            propInfo.PropertyType = typeof (TValue);
            _propertyMapOptions = new PropertyMapOptions<T>(propInfo);
            _mappings.Add(name, propInfo);
            return _propertyMapOptions;
        }

        protected void MapAsNullable<TValue>(Expression<Func<T, TValue>> getterExpression, string name)
        {
            var propInfo = CreatePropertyMapInfo(getterExpression, PropertyKind.Nullable);
            propInfo.PropertyType = typeof (TValue);
            
            _mappings.Add(name, propInfo);
        }

        protected IInheritanceMapOptions MapAsCollection<TValue>(Expression<Func<T, TValue>> getterExpression, string name) where TValue: IEnumerable
        {
            var collectionType = typeof (TValue);
            var propertyKind = GetPropertyKind(collectionType);

            var propInfo = CreatePropertyMapInfo(getterExpression, propertyKind);

            propInfo.PropertyType = TypeHelper.GetCollectionElementType(collectionType);
            _propertyMapOptions = new PropertyMapOptions<T>(propInfo);
            _mappings.Add(name, propInfo);
            return _propertyMapOptions;
        }

        protected IInheritanceMapOptions MapAsDictionary<TValue>(Expression<Func<T, TValue>> getterExpression, string name) where TValue : IDictionary
        {
            var propInfo = CreatePropertyMapInfo(getterExpression, PropertyKind.Dictionary);
            propInfo.PropertyType = typeof(TValue);
            _propertyMapOptions = new PropertyMapOptions<T>(propInfo);
            _mappings.Add(name, propInfo);
            return _propertyMapOptions;
        }

        private static PropertyKind GetPropertyKind(Type collectionType)
        {
            var propertyKind = PropertyKind.Collection;
            if (collectionType.IsArray)
            {
                propertyKind = PropertyKind.Array;
            }
            return propertyKind;
        }


        private PropertyMapInfo<T> CreatePropertyMapInfo<TValue>(Expression<Func<T, TValue>> getterExpression, PropertyKind propertyKind)
        {
            var set = PropertyExpressionHelper.InitializeSetter(getterExpression);
            var get = PropertyExpressionHelper.InitializeGetter(getterExpression);
            var setter = new Action<T, object>((o, v) => set(o, (TValue) v));
            var getter = new Func<T, object>(o => get(o));

            var propInfo = new PropertyMapInfo<T>
                {
                    Setter = setter,
                    Getter = getter,
                    PropertyKind = propertyKind
                };
            return propInfo;
        }
    }
}
