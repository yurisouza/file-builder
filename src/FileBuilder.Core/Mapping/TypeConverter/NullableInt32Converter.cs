using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class NullableInt32Converter : NullableInnerConverter<Int32>
    {
        public NullableInt32Converter()
            : base(new Int32Converter())
        {
        }

        public NullableInt32Converter(IFormatProvider formatProvider)
            : base(new Int32Converter(formatProvider))
        {
        }

        public NullableInt32Converter(IFormatProvider formatProvider, NumberStyles numberStyles)
            : base(new Int32Converter(formatProvider, numberStyles))
        {
        }
    }
}
