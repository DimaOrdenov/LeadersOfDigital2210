using System;
using System.Linq;
using System.Reflection;

namespace LeadersOfDigital.Helpers
{
    public static class AttributeExtensions
    {
        public static TProperty GetPropertyAttributeValue<T, TAttribute, TProperty>(this T type, Func<TAttribute, TProperty> expression, TProperty defaultValue = default)
            where TAttribute : Attribute
            where TProperty : class
        {
            var attribute = GetAttribute<T, TAttribute>(type);

            return attribute != null ? expression(attribute) : defaultValue;
        }

        private static TAttribute GetAttribute<T, TAttribute>(T type)
            where TAttribute : Attribute =>
            type.GetType().GetField(type.ToString()).GetCustomAttributes().FirstOrDefault(x => x is TAttribute) as TAttribute;
    }
}
