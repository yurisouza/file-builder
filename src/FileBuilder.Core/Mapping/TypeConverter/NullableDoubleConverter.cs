using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class NullableDoubleConverter : NullableInnerConverter<Double>
    {
        public NullableDoubleConverter()
            : base(new DoubleConverter())
        {
        }
        
        public NullableDoubleConverter(IFormatProvider formatProvider)
            : base(new DoubleConverter(formatProvider))
        {
        }

        public NullableDoubleConverter(IFormatProvider formatProvider, NumberStyles numberStyles)
            : base(new DoubleConverter(formatProvider, numberStyles))
        {
        }
    }
}