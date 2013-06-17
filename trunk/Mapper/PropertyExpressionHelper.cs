using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Mapper
{
    internal static class PropertyExpressionHelper
    {
        public static Func<TContainer, TProperty> InitializeGetter<TContainer, TProperty>(Expression<Func<TContainer, TProperty>> getterExpression) where TContainer : class
        {
            return getterExpression.Compile();
        }

        public static Action<TContainer, TProperty> InitializeSetter<TContainer, TProperty>(Expression<Func<TContainer, TProperty>> getter) where TContainer : class
        {
            var propertyInfo = (getter.Body as MemberExpression).Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("The expression is not a property","getter");
            }
            var instance = Expression.Parameter(typeof (TContainer), "instance");
            var parameter = Expression.Parameter(typeof (TProperty), "param");

            return Expression.Lambda<Action<TContainer, TProperty>>(
                Expression.Call(instance, propertyInfo.GetSetMethod(), parameter),
                new[] {instance, parameter}).Compile();
        }
    }
}