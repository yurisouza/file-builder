namespace FileBuilder.Core.Mapping
{
    public interface IPropertyMapping<in TEntity, in TProperty>
    {
        bool TryMapValue(TEntity entity, TProperty value);
    }
}