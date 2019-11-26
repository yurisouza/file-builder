using System;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public interface ITypeConverter
    {

    }

    public interface ITypeConverter<TTargetType> : ITypeConverter
    {
        bool TryConvert(string value, out TTargetType result);

        Type TargetType { get; }
    }

    public interface IArrayTypeConverter<TTargetType> : ITypeConverter
    {
        bool TryConvert(string[] value, out TTargetType result);

        Type TargetType { get; }
    }
}
