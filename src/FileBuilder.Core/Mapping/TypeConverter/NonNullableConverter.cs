using System;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public abstract class NonNullableConverter<TTargetType> : BaseConverter<TTargetType>
    {
        public override bool TryConvert(string value, out TTargetType result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default(TTargetType);

                return false;
            }

            return InternalConvert(value, out result);
            
        }

        protected abstract bool InternalConvert(string value, out TTargetType result);
    }
}
