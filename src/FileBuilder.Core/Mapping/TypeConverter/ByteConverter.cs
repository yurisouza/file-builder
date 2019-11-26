using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class ByteConverter : NonNullableConverter<Byte>
    {
        private readonly IFormatProvider formatProvider;
        private readonly NumberStyles numberStyles;

        public ByteConverter()
            : this(CultureInfo.InvariantCulture)
        {
        }

        public ByteConverter(IFormatProvider formatProvider)
            : this(formatProvider, NumberStyles.Integer)
        {
        }

        public ByteConverter(IFormatProvider formatProvider, NumberStyles numberStyles)
        {
            this.formatProvider = formatProvider;
            this.numberStyles = numberStyles;
        }


        protected override bool InternalConvert(string value, out Byte result)
        {
            return Byte.TryParse(value, numberStyles, formatProvider, out result);
        }
    }
}
