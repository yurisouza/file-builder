using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class NullableSByteConverter : NullableInnerConverter<SByte>
    {
        public NullableSByteConverter()
            : base(new SByteConverter())
        {
        }

        public NullableSByteConverter(IFormatProvider formatProvider)
            : base(new SByteConverter(formatProvider))
        {
        }

        public NullableSByteConverter(IFormatProvider formatProvider, NumberStyles numberStyles)
            : base(new SByteConverter(formatProvider, numberStyles))
        {
        }
    }
}