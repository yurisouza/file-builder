using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class SByteConverter : NonNullableConverter<SByte>
    {
        private readonly IFormatProvider formatProvider;
        private readonly NumberStyles numberStyles;

        public SByteConverter()
            : this(CultureInfo.InvariantCulture)
        {
        }

        public SByteConverter(IFormatProvider formatProvider)
            : this(formatProvider, NumberStyles.Integer)
        {
        }

        public SByteConverter(IFormatProvider formatProvider, NumberStyles numberStyles)
        {
            this.formatProvider = formatProvider;
            this.numberStyles = numberStyles;
        }


        protected override bool InternalConvert(string value, out SByte result)
        {
            return SByte.TryParse(value, numberStyles, formatProvider, out result);
        }
    }
}
