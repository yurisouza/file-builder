using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public abstract class NullableInnerConverter<TTargetType> : NullableConverter<TTargetType?>
        where TTargetType : struct
    {
        private readonly NonNullableConverter<TTargetType> internalConverter;

        public NullableInnerConverter(NonNullableConverter<TTargetType> internalConverter)
        {
            this.internalConverter = internalConverter;
        }

        protected override bool InternalConvert(string value, out TTargetType? result)
        {
            result = default(TTargetType?);

            TTargetType innerConverterResult;

            if (internalConverter.TryConvert(value, out innerConverterResult))
            {
                result = innerConverterResult;

                return true;
            }

            return false;
        }
    }
}
