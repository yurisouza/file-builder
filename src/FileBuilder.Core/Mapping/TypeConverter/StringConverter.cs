using System;
using System.Globalization;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class StringConverter : BaseConverter<string>
    {
        public override bool TryConvert(string value, out string result)
        {
            result = value;

            return true;
        }
    }
}