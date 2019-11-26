using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class NullableUInt64Converter : NullableInnerConverter<UInt64>
    {
        public NullableUInt64Converter()
            : base(new UInt64Converter())

        {
        }

        public NullableUInt64Converter(IFormatProvider formatProvider)
            : base(new UInt64Converter(formatProvider))
        {
        }

        public NullableUInt64Converter(IFormatProvider formatProvider, NumberStyles numberStyles)
            : base(new UInt64Converter(formatProvider, numberStyles))
        {
        }
    }
}
