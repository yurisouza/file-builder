using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class Int16Converter : NonNullableConverter<Int16>
    {
        private readonly IFormatProvider formatProvider;
        private readonly NumberStyles numberStyles;

        public Int16Converter()
            : this(CultureInfo.InvariantCulture)
        {
        }

        public Int16Converter(IFormatProvider formatProvider)
            : this(formatProvider, NumberStyles.Integer)
        {
        }

        public Int16Converter(IFormatProvider formatProvider, NumberStyles numberStyles)
        {
            this.formatProvider = formatProvider;
            this.numberStyles = numberStyles;
        }

        protected override bool InternalConvert(string value, out Int16 result)
        {
            return Int16.TryParse(value, numberStyles, formatProvider, out result);
        }
    }
}