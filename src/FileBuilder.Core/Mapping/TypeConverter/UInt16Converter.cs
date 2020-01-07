using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class UInt16Converter : NonNullableConverter<UInt16>
    {
        private readonly IFormatProvider formatProvider;
        private readonly NumberStyles numberStyles;

        public UInt16Converter()
            : this(CultureInfo.InvariantCulture)
        {
        }

        public UInt16Converter(IFormatProvider formatProvider)
            : this(formatProvider, NumberStyles.Integer)
        {
        }

        public UInt16Converter(IFormatProvider formatProvider, NumberStyles numberStyles)
        {
            this.formatProvider = formatProvider;
            this.numberStyles = numberStyles;
        }

        protected override bool InternalConvert(string value, out UInt16 result)
        {
            return UInt16.TryParse(value, numberStyles, formatProvider, out result);
        }
    }
}
