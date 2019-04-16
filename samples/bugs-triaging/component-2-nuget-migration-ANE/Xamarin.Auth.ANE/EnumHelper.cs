using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LeanKit.Core.Utility
{
    public static class EnumHelper
    {
		static EnumHelper()
		{
		}

        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes =
                  (DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        public static long GetId(this Enum value)
        {
            var fi = Convert.ToInt32(value);
            return fi;
        }

        public static IEnumerable<T> EnumToList<T>()
            where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}