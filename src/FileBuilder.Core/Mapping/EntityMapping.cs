using FileBuilder.Core.Mapping.TypeConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FileBuilder.Core.Mapping
{
    public class EntityMapping<TEntity> where TEntity : class, new()
    {
        private class IndexToPropertyMapping
        {
            public string ColumnName { get; set; }

            public IPropertyMapping<TEntity, string> PropertyMapping { get; set; }

            public override string ToString()
            {
                return $"IndexToPropertyMapping (ColumnPosition = {ColumnName}, PropertyMapping = {PropertyMapping}";
            }
        }

        private readonly List<IndexToPropertyMapping> _propertyMappings;
        private readonly ITypeConverterProvider _typeConverterProvider;

        public EntityMapping()
            : this(new TypeConverterProvider())
        {
        }

        public EntityMapping(ITypeConverterProvider typeConverterProvider)
        {
            _typeConverterProvider = typeConverterProvider;
            _propertyMappings = new List<IndexToPropertyMapping>();
        }

        public PropertyMapping<TEntity, TProperty> MapProperty<TProperty>(string columnName, Expression<Func<TEntity, TProperty>> property)
        {
            return MapProperty(columnName, property, _typeConverterProvider.Resolve<TProperty>());
        }

        public PropertyMapping<TEntity, TProperty> MapProperty<TProperty>(string columnName, Expression<Func<TEntity, TProperty>> property, ITypeConverter<TProperty> typeConverter)
        {
            if (_propertyMappings.Any(x => x.ColumnName == columnName))
                throw new InvalidOperationException($"Duplicate mapping for column index {columnName}");

            var propertyMapping = new PropertyMapping<TEntity, TProperty>(property, typeConverter);

            AddPropertyMapping(columnName, propertyMapping);

            return propertyMapping;
        }

        public TEntity Map(IDictionary<string, string> line)
        {
            var entity = new TEntity();

            foreach (var property in _propertyMappings)
            {
                if(line.TryGetValue(property.ColumnName, out var word))
                    property.PropertyMapping.TryMapValue(entity, word);
            }

            return entity;
        }

        private void AddPropertyMapping<TProperty>(string columnName, PropertyMapping<TEntity, TProperty> propertyMapping)
        {
            var indexToPropertyMapping = new IndexToPropertyMapping
            {
                ColumnName = columnName,
                PropertyMapping = propertyMapping
            };

            _propertyMappings.Add(indexToPropertyMapping);
        }

    }
}
