using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auto_highlighter_iam.Extentions
{
    public static class EnumExtentions
    {
        public static bool ToBool<T>(this T value)
        {
            return value.ToString() == "1" || value.ToString() == "true";
        }
        public static IEnumerable<bool> CastBool<T>(this IEnumerable<T> data)
        {
            List<bool> newData = new();
            foreach (T value in data)
            {
                newData.Add(value.ToBool());
            }
            return newData;
        }
        public static IEnumerable<bool> CastBool(this Array data)
        {
            List<bool> newData = new();
            foreach (var value in data)
            {
                newData.Add(value.ToBool());
            }
            return newData;
        }
    }
}
