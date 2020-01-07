using FileBuilder.Core.Mapping.TypeConverter;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FileBuilder.Core.Mapping
{
    public class PropertyMapping<TEntity, TProperty> : IPropertyMapping<TEntity, string>
        where TEntity : class, new()
    {
        private readonly string propertyName;
        private readonly ITypeConverter<TProperty> propertyConverter;
        private readonly Action<TEntity, TProperty> propertySetter;

        public PropertyMapping(Expression<Func<TEntity, TProperty>> property, ITypeConverter<TProperty> typeConverter)
        {
            propertyConverter = typeConverter;
            propertyName = ReflectionUtils.GetPropertyNameFromExpression(property);
            propertySetter = ReflectionUtils.CreateSetter<TEntity, TProperty>(property);
        }

        public bool TryMapValue(TEntity entity, string value)
        {
            if (!propertyConverter.TryConvert(value, out var convertedValue))
            {
                return false;
            }

            propertySetter(entity, convertedValue);

            return true;
        }

        public override string ToString()
        {
            return $"CsvPropertyMapping (PropertyName = {propertyName}, Converter = {propertyConverter})";
        }
    }
}
