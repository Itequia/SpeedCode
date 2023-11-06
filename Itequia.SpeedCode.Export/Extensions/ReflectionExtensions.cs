using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Export.Extensions
{
    public static class ReflectionExtensions
    {
        public static object GetPropertyValue(this object obj, string name)
        {
            foreach (string part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part.DeCamelCase(),
                          BindingFlags.FlattenHierarchy |
                          BindingFlags.Instance |
                          BindingFlags.Public);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }

            return obj;
        }

        public static T GetPropertyValue<T>(this object obj, string name)
        {
            object result = GetPropertyValue(obj, name);
            if (result == null) { return default; }

            // throws InvalidCastException if types are incompatible
            return (T)result;
        }

        public static void SetWithReflection<T, P>(this T obj, Expression<Func<T, P>> key, P value)
        {
            var propertyName = key.GetPropertyName();
            var objType = obj.GetType();

            while (objType.FullName != "System.Object")
            {
                try
                {
                    objType.GetProperties().Single(x => x.Name == propertyName).SetValue(obj, value, null);
                }
                catch //If we can't set it in the obj we will try in its parent
                {
                    objType = objType.BaseType;
                    continue;
                }
                break;
            }
        }

        public static string GetPropertyName<TSource, TProperty>(this Expression<Func<TSource, TProperty>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression), "propertyRefExpr is null.");

            MemberExpression memberExpr = expression.Body as MemberExpression;

            if (memberExpr == null)
            {
                if (expression.Body is UnaryExpression unaryExpr && unaryExpr.NodeType == ExpressionType.Convert)
                    memberExpr = unaryExpr.Operand as MemberExpression;
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property) return memberExpr.Member.Name;

            throw new ArgumentException("No property reference expression was found.", nameof(expression));
        }
    }
}
