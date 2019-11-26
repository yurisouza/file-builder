using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class UInt64Converter : NonNullableConverter<UInt64>
    {
        private readonly IFormatProvider formatProvider;
        private readonly NumberStyles numberStyles;

        public UInt64Converter()
            : this(CultureInfo.InvariantCulture)
        {
        }

        public UInt64Converter(IFormatProvider formatProvider)
            : this(formatProvider, NumberStyles.Integer)
        {
        }

        public UInt64Converter(IFormatProvider formatProvider, NumberStyles numberStyles)
        {
            this.formatProvider = formatProvider;
            this.numberStyles = numberStyles;
        }

        protected override bool InternalConvert(string value, out UInt64 result)
        {
            return UInt64.TryParse(value, numberStyles, formatProvider, out result);
        }
    }
}