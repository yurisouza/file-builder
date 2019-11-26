using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class UInt32Converter : NonNullableConverter<UInt32>
    {
        private readonly IFormatProvider formatProvider;
        private readonly NumberStyles numberStyles;

        public UInt32Converter()
            : this(CultureInfo.InvariantCulture)
        {
        }

        public UInt32Converter(IFormatProvider formatProvider)
            : this(formatProvider, NumberStyles.Integer)
        {
        }

        public UInt32Converter(IFormatProvider formatProvider, NumberStyles numberStyles)
        {
            this.formatProvider = formatProvider;
            this.numberStyles = numberStyles;
        }

        protected override bool InternalConvert(string value, out UInt32 result)
        {
            return UInt32.TryParse(value, numberStyles, formatProvider, out result);
        }
    }
}
