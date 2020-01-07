using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class NullableGuidConverter : NullableInnerConverter<Guid>
    {
        public NullableGuidConverter()
            : base(new GuidConverter())
        {
        }

        public NullableGuidConverter(string format)
            : base(new GuidConverter(format))
        {
        }
    }
}