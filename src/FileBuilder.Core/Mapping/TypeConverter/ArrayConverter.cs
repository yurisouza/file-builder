﻿using System;

namespace FileBuilder.Core.Mapping.TypeConverter
{
    public class ArrayConverter<TTargetType> : IArrayTypeConverter<TTargetType[]>
    {
        private readonly ITypeConverter<TTargetType> internalConverter;

        public ArrayConverter(ITypeConverter<TTargetType> internalConverter)
        {
            this.internalConverter = internalConverter;
        }

        public bool TryConvert(string[] values, out TTargetType[] result)
        {
            result = new TTargetType[values.Length];

            for(int pos = 0; pos < values.Length; pos++)
            {
                if (!internalConverter.TryConvert(values[pos], out TTargetType element))
                {
                    return false;
                }

                result[pos] = element;
            }

            return true;
        }

        public Type TargetType => typeof(TTargetType[]);
    }
}
