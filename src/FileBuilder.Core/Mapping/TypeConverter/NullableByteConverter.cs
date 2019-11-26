using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class NullableByteConverter : NullableInnerConverter<byte>
    {
        public NullableByteConverter()
            : base(new ByteConverter())
        {
        }

        public NullableByteConverter(IFormatProvider formatProvider)
            : base(new ByteConverter(formatProvider))
        {
        }

        public NullableByteConverter(IFormatProvider formatProvider, NumberStyles numberStyles)
            : base(new ByteConverter(formatProvider, numberStyles))
        {
        }
    }
}