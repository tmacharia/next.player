using System;
using System.Linq.Expressions;
using System.Reflection;
using Common;

namespace Next.PCL.Extensions
{
    public static class ExpressionExts
    {
        public static TProp GetPropValue<TClass, TProp>(this TClass @class, Expression<Func<TClass, TProp>> propSelector)
            where TClass : class
        {
            return @class.GetPropertyValue<TClass, TProp>(propSelector.GetPropName());
        }
        public static string GetPropName<TClass, TProp>(this Expression<Func<TClass, TProp>> exp)
        {
            if (exp == null)
                return string.Empty;
            if (exp.Body is MemberExpression m)
                return m.Member.Name;
            else if (exp.Body is UnaryExpression ue)
                return ((MemberExpression)ue.Operand).Member.Name;
            return string.Empty;
        }
        public static Type GetPropType<TClass>(this Expression<Func<TClass, object>> exp)
            where TClass : class
        {
            if (exp == null)
                return null;
            if (exp.Body is MemberExpression m) {
                return ((PropertyInfo)m.Member).PropertyType;
            }
            else if (exp.Body is UnaryExpression ue) {
                if (ue.Operand is MemberExpression mue) {
                    return ((PropertyInfo)mue.Member).PropertyType;
                }
            }
            return null;
        }
        public static void SetPropValue<TClass, TProp>(this TClass @class, Expression<Func<TClass, TProp>> propertySelector, TProp newValue)
            where TClass : class
        {
            if (@class == null)
                return;

            string prop = propertySelector.GetPropName();
            if (prop.IsValid())
                @class.SetPropertyValue(prop, newValue);
        }

    }
}