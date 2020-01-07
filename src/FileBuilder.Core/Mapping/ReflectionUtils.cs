using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FileBuilder.Core.Mapping
{
    public static class ReflectionUtils
    {
        public static Action<TEntity, TProperty> CreateSetter<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> property)
        {
            PropertyInfo propertyInfo = GetProperty(property);

            ParameterExpression instance = Expression.Parameter(typeof(TEntity), "instance");
            ParameterExpression parameter = Expression.Parameter(typeof(TProperty), "param");

            var setMethod = propertyInfo.GetSetMethod();
            if (setMethod == null)
            {
                throw new InvalidOperationException($"Unable to map to property '{property.Body}' because it does not contain a setter.");
            }
            return Expression.Lambda<Action<TEntity, TProperty>>(
                Expression.Call(instance, setMethod, parameter),
                new ParameterExpression[] { instance, parameter }).Compile();
        }

        public static PropertyInfo GetProperty<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            var member = GetMemberExpression(expression).Member;
            var property = member as PropertyInfo;

            if (property == null)
            {
                string message = $"Member with Name '{member.Name}' is not a property.";

                throw new InvalidOperationException(message);
            }
            return property;
        }

        public static string GetPropertyNameFromExpression<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            var member = GetMemberExpression(expression).Member;

            return member.Name;
        }

        public static bool IsEnum(Type type)
        {
            return typeof(Enum).IsAssignableFrom(type);
        }

        private static MemberExpression GetMemberExpression<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException("Not a member access", nameof(expression));
            }

            return memberExpression;
        }
    }
}