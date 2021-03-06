﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Mapper.Helpers
{
    internal static class PropertyExpressionHelper
    {
        public static Func<TContainer, TProperty> InitializeGetter<TContainer, TProperty>(Expression<Func<TContainer, TProperty>> getterExpression) 
        {
            return getterExpression.Compile();
        }

        public static Action<TContainer, TProperty> InitializeSetter<TContainer, TProperty>(Expression<Func<TContainer, TProperty>> getter) 
        {
            var propertyInfo = (getter.Body as MemberExpression).Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("The expression is not a property","getter");
            }
            var instance = Expression.Parameter(typeof (TContainer), "instance");
            var parameter = Expression.Parameter(typeof (TProperty), "param");

            return Expression.Lambda<Action<TContainer, TProperty>>(
                Expression.Call(instance, propertyInfo.GetSetMethod(true), parameter),
                new[] {instance, parameter}).Compile();
        }
    }
}