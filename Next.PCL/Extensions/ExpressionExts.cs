using System;
using System.Collections.Generic;
using System.Linq;
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
            var stack = new Stack<string>();
            Expression expression1 = exp.Body;
            while (expression1 != null)
            {
                if (expression1.NodeType == ExpressionType.Call)
                {
                    var methodCallExpression = (MethodCallExpression)expression1;
                    if (IsSingleArgumentIndexer(methodCallExpression))
                    {
                        stack.Push(string.Empty);
                        expression1 = methodCallExpression.Object;
                    }
                    else
                        break;
                }
                else if (expression1.NodeType == ExpressionType.ArrayIndex)
                {
                    var binaryExpression = (BinaryExpression)expression1;
                    stack.Push(string.Empty);
                    expression1 = binaryExpression.Left;
                }
                else if (expression1.NodeType == ExpressionType.MemberAccess)
                {
                    var memberExpression = (MemberExpression)expression1;
                    stack.Push("." + memberExpression.Member.Name);
                    expression1 = memberExpression.Expression;
                }
                else if (expression1.NodeType == ExpressionType.Parameter)
                {
                    stack.Push(string.Empty);
                    expression1 = null;
                }
                else if (expression1.NodeType == ExpressionType.Convert)
                {
                    var memberExp = ((UnaryExpression)expression1).Operand as MemberExpression;
                    stack.Push("." + memberExp.Member.Name);
                    expression1 = memberExp.Expression;
                }
                else
                    break;
            }
            if (stack.Count > 0 && string.Equals(stack.Peek(), ".model", StringComparison.OrdinalIgnoreCase))
                stack.Pop();
            if (stack.Count <= 0)
                return string.Empty;
            string s = (stack).Aggregate(((left, right) => left + right)).TrimStart(new[] { '.' });
            return s;
        }
        public static TProp GetPropValue<TClass, TProp>(this TClass @class, Expression<Func<TClass>> propSelector)
            where TClass : class
        {
            return @class.GetPropertyValue<TClass, TProp>(propSelector.GetPropName());
        }
        public static string GetPropName<TClass>(this Expression<Func<TClass>> expression)
        {
            var stack = new Stack<string>();
            Expression expression1 = expression.Body;
            while (expression1 != null)
            {
                if (expression1.NodeType == ExpressionType.Call)
                {
                    var methodCallExpression = (MethodCallExpression)expression1;
                    if (IsSingleArgumentIndexer(methodCallExpression))
                    {
                        stack.Push(string.Empty);
                        expression1 = methodCallExpression.Object;
                    }
                    else
                        break;
                }
                else if (expression1.NodeType == ExpressionType.ArrayIndex)
                {
                    var binaryExpression = (BinaryExpression)expression1;
                    stack.Push(string.Empty);
                    expression1 = binaryExpression.Left;
                }
                else if (expression1.NodeType == ExpressionType.MemberAccess)
                {
                    var memberExpression = (MemberExpression)expression1;
                    stack.Push("." + memberExpression.Member.Name);
                    expression1 = memberExpression.Expression;
                }
                else if (expression1.NodeType == ExpressionType.Parameter)
                {
                    stack.Push(string.Empty);
                    expression1 = null;
                }
                else if (expression1.NodeType == ExpressionType.Convert)
                {
                    var memberExp = ((UnaryExpression)expression1).Operand as MemberExpression;
                    stack.Push("." + memberExp.Member.Name);
                    expression1 = memberExp.Expression;
                }
                else
                    break;
            }
            if (stack.Count > 0 && string.Equals(stack.Peek(), ".model", StringComparison.OrdinalIgnoreCase))
                stack.Pop();
            if (stack.Count <= 0)
                return string.Empty;
            string s = (stack).Aggregate(((left, right) => left + right)).TrimStart(new[] { '.' });
            Console.WriteLine(s);
            return s;
        }
        private static bool IsSingleArgumentIndexer(Expression expression)
        {
            var methodExpression = expression as MethodCallExpression;
            if (methodExpression == null || methodExpression.Arguments.Count != 1)
                return false;
            return (methodExpression.Method.DeclaringType.GetDefaultMembers()).OfType<PropertyInfo>().Any((p => p.GetGetMethod() == methodExpression.Method));
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
        public static string GetPropName<TClass, TProp>(this TClass @class, Expression<Func<TClass, TProp>> propertySelector)
            where TClass : class
        {
            if (@class == null)
                return "";

            string prop = propertySelector.GetPropName();
            return prop;
        }
    }
}