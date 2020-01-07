using System;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public abstract class BaseConverter<TTargetType> : ITypeConverter<TTargetType>
    {
        public abstract bool TryConvert(string value, out TTargetType result);

        public Type TargetType => typeof(TTargetType);
    }
}
