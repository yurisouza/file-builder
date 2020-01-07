using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class NullableBoolConverter : NullableInnerConverter<bool>
    {
        public NullableBoolConverter()
            : base(new BoolConverter())
        {
        }

        public NullableBoolConverter(string trueValue, string falseValue, StringComparison stringComparism)
            : base(new BoolConverter(trueValue, falseValue, stringComparism))
        {
        }
    }
}