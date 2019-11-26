namespace FileBuilder.Core.Mapping.TypeConverter
{
    public interface ITypeConverterProvider
    {
        ITypeConverter<TTargetType> Resolve<TTargetType>();

        IArrayTypeConverter<TTargetType> ResolveCollection<TTargetType>();
    }
}
