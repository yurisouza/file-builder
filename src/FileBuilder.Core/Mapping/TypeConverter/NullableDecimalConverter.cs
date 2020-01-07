using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class NullableDecimalConverter : NullableInnerConverter<Decimal>
    {
        public NullableDecimalConverter()
            : base(new DecimalConverter())
        {
        }

        public NullableDecimalConverter(IFormatProvider formatProvider)
            : base(new DecimalConverter(formatProvider))
        {
        }

        public NullableDecimalConverter(IFormatProvider formatProvider, NumberStyles numberStyles)
            : base(new DecimalConverter(formatProvider, numberStyles))
        {
        }
    }
}