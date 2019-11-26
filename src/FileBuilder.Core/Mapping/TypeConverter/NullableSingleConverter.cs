using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class NullableSingleConverter : NullableInnerConverter<Single>
    {
        public NullableSingleConverter()
            : base(new SingleConverter())
        {
        }

        public NullableSingleConverter(IFormatProvider formatProvider)
            : base(new SingleConverter(formatProvider))
        {
        }

        public NullableSingleConverter(IFormatProvider formatProvider, NumberStyles numberStyles)
            : base(new SingleConverter(formatProvider, numberStyles))
        {
        }
   }
}