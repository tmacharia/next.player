using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Next.PCL.Extensions
{
    public static class ReflectionExts
    {
        private static IDictionary<Type, List<PropertyDescriptor>> _cache =
            new ConcurrentDictionary<Type, List<PropertyDescriptor>>();

        private static PropertyDescriptor[] GetPropertyDescriptors(Type type)
        {
            PropertyDescriptorCollection props;
            if (_cache.ContainsKey(type))
                return _cache[type].ToArray();
            else
                props = TypeDescriptor.GetProperties(type);

            List<PropertyDescriptor> properties = new List<PropertyDescriptor>();
            for (int i = 0; i < props.Count; i++)
            {
                properties.Add(props[i]);
            }
            _cache.Add(type, properties);
            return _cache[type].ToArray();
        }

        private static PropertyDescriptor[] GetPropertyDescriptors<TClass>()
            where TClass : class
        {
            return GetPropertyDescriptors(typeof(TClass));
        }
        private static PropertyDescriptor GetDescriptor(Type type, string prop)
            => GetPropertyDescriptors(type).FirstOrDefault(x => x.Name == prop);

        private static PropertyDescriptor GetDescriptor<TClass>(this TClass @class, string prop)
            where TClass : class => GetPropertyDescriptors<TClass>().FirstOrDefault(x => x.Name == prop);

        public static TProperty GetPropValue2<TClass, TProperty>(this TClass @class, Expression<Func<TClass, TProperty>> exp)
            where TClass : class
        {
            string propertyName = exp.GetPropName();
            var obj = @class.GetPropValue2(propertyName);
            if(obj != null)
            {
                return (TProperty)obj;
            }
            return default(TProperty);
        }
        public static TProperty GetPropValue2<TClass, TProperty>(this TClass @class, string propertyName)
            where TClass : class
        {
            var obj = @class.GetPropValue2(propertyName);
            if (obj != null)
            {
                return (TProperty)obj;
            }
            return default(TProperty);
        }
        public static object GetPropValue2<TClass>(this TClass @class, string propertyName)
            where TClass : class
        {
            if (!propertyName.Contains('.'))
            {
                PropertyDescriptor prop = GetDescriptor(@class, propertyName);
                if (prop != null)
                {
                    return prop.GetValue(@class);
                }
            }
            else
            {
                var parts = propertyName.Split('.');
                object obj = @class;
                for (int i = 0; i < parts.Length; i++)
                {
                    PropertyDescriptor prop = GetDescriptor(obj.GetType(), parts[i]);
                    obj = prop.GetValue(obj);
                }
                return obj;
            }
            return null;
        }
    }
}