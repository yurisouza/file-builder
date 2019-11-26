﻿using System;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class BoolConverter : NonNullableConverter<bool>
    {
        private readonly string trueValue;
        private readonly string falseValue;
        private readonly StringComparison stringComparism;

        public BoolConverter()
            : this("true", "false", StringComparison.OrdinalIgnoreCase) { }

        public BoolConverter(string trueValue, string falseValue, StringComparison stringComparism)
        {
            this.trueValue = trueValue;
            this.falseValue = falseValue;
            this.stringComparism = stringComparism;
        }

        protected override bool InternalConvert(string value, out bool result)
        {
            result = false;

            if(string.Equals(trueValue, value, stringComparism)) 
            {
                result = true;

                return true;
            }

            if (string.Equals(falseValue, value, stringComparism))
            {
                result = false;

                return true;
            }

            return false;
        }
    }
}
