using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class DoubleConverter : NonNullableConverter<Double>
    {
        private readonly IFormatProvider formatProvider;
        private readonly NumberStyles numberStyles;

        public DoubleConverter()
            : this(CultureInfo.InvariantCulture)
        {
        }

        public DoubleConverter(IFormatProvider formatProvider)
            : this(formatProvider, NumberStyles.Float | NumberStyles.AllowThousands)
        {
        }

        public DoubleConverter(IFormatProvider formatProvider, NumberStyles numberStyles)
        {
            this.formatProvider = formatProvider;
            this.numberStyles = numberStyles;
        }

        protected override bool InternalConvert(string value, out Double result)
        {
            return Double.TryParse(value, numberStyles, formatProvider, out result);
        }
    }
}
