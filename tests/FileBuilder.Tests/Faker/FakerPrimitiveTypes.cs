using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileBuilder.Tests.Faker
{
    public class FakerPrimitiveTypes
    {
        /// <summary>
        /// Get values of a enum
        /// </summary>
        /// <typeparam name="TEnum">Enum</typeparam>
        /// <returns></returns>
        public static List<TEnum> GetEnumValues<TEnum>() where TEnum : struct
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
        }

        /// <summary>
        /// Generate Text With Min = 1 (default) and Max of caracteres
        /// </summary>
        ///// <param name="max">Size max</param>
        ///// <param name="min">Size min</param>
        /// <returns>string</returns>
        public static string GetSampleText(int min = 1, int max = 0)
        {
            if (max == 0)
                max = min + new Random().Next(2015);

            if (min > max)
                throw new Exception("The minimum number can't be greater than maximum number.");

            return new Faker<string>()
                .CustomInstantiator(f => f.Lorem.Sentence(max).Substring(1, (f.Random.Number(min, max))).Trim());
        }

        /// <summary>
        /// Generate Text between 1 and Max of caracteres
        /// </summary>
        /// <param name="max">Size max</param>
        /// <returns>string</returns>
        public static string GetSampleText(int max)
        {
            return new Faker<string>()
                .CustomInstantiator(f => f.Lorem.Sentence(max).Substring(1, (f.Random.Number(1, max))).Trim());
        }

        /// <summary>
        /// Generate a random boolean
        /// </summary>
        /// <returns></returns>
        public static bool RandomBoolean()
        {
            return RandomInt(1) == 0;
        }

        /// <summary>
        /// Get a random value of Enum
        /// </summary>
        /// <typeparam name="TEnum">Enum</typeparam>
        /// <returns></returns>
        public static TEnum RandomEnumValue<TEnum>() where TEnum : struct
        {
            var values = Enum.GetValues(typeof(TEnum));
            return (TEnum)values.GetValue(new Random().Next(values.Length));
        }

        /// <summary>
        /// Generate a random integer beteween Min(Default = 0) and Max(Default = 1)  inclusive
        /// </summary>
        /// <param name="min">Value min.</param>
        /// <param name="max">Value max.</param>
        /// <returns></returns>
        public static int RandomInt(int min = 0, int max = 1)
        {
            if (min >= max)
                throw new Exception("The minimum number can't be greater or equal than maximum number.");

            return new Random().Next(min, ++max);
        }

        /// <summary>
        /// Generate a random integer beteween 1 and Max inclusive
        /// </summary>
        /// <param name="max">Valur max.</param>
        /// <returns>Random integer</returns>
        public static int RandomInt(int max)
        {
            if (max == 0)
                throw new Exception("The maximum number must greater than 0.");

            return new Random().Next(1, ++max);
        }
    }
}