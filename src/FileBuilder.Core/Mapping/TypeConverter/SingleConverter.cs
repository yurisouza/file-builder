using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class SingleConverter : NonNullableConverter<Single>
    {
        private readonly IFormatProvider formatProvider;
        private readonly NumberStyles numberStyles;

        public SingleConverter()
            : this(CultureInfo.InvariantCulture)
        {
        }

        public SingleConverter(IFormatProvider formatProvider)
            : this(formatProvider, NumberStyles.Float | NumberStyles.AllowThousands)
        {
        }

        public SingleConverter(IFormatProvider formatProvider, NumberStyles numberStyles)
        {
            this.formatProvider = formatProvider;
            this.numberStyles = numberStyles;
        }

        protected override bool InternalConvert(string value, out Single result)
        {
            return Single.TryParse(value, numberStyles, formatProvider, out result);
        }
    }
}