using System;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class GuidConverter : NonNullableConverter<Guid>
    {
        private readonly string format;

        public GuidConverter()
            : this(string.Empty)
        {
        }

        public GuidConverter(string format)
        {
            this.format = format;
        }

        protected override bool InternalConvert(string value, out Guid result)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return Guid.TryParse(value, out result);
            }
            return Guid.TryParseExact(value, format, out result);
        }
    }
}
