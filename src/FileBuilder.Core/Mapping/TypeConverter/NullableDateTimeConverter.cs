using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class NullableDateTimeConverter : NullableInnerConverter<DateTime>
    {
        public NullableDateTimeConverter()
            : base(new DateTimeConverter())
        {
        }
        
        public NullableDateTimeConverter(string dateTimeFormat)
            : base(new DateTimeConverter(dateTimeFormat))
        {
        }

        public NullableDateTimeConverter(string dateTimeFormat, IFormatProvider formatProvider)
            : base(new DateTimeConverter(dateTimeFormat, formatProvider))
        {
        }

        public NullableDateTimeConverter(string dateTimeFormat, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
            : base(new DateTimeConverter(dateTimeFormat, formatProvider, dateTimeStyles))
        {
        }
    }
}
