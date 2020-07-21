/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Nav2018.Extensions
{
    public static class StringExtensions
    {

        public static string NotNull(this string value)
        {
            if (value == null)
                return "";
            return value;
        }

        public static string Merge(params string[] values)
        {
            string mergedText = "";
            bool textIsEmpty = true;
            for (int i=0; i<values.Length; i++)
            {
                if (!String.IsNullOrWhiteSpace(values[i]))
                {
                    if (textIsEmpty)
                    {
                        mergedText = values[i];
                        textIsEmpty = false;
                    }
                    else
                        mergedText = mergedText + " " + values[i];
                }
            }
            return mergedText;
        }

        public static bool EqualsOrEmpty(this string value, string value2)
        {
            return ((String.IsNullOrWhiteSpace(value)) || (value.Equals(value2, StringComparison.CurrentCultureIgnoreCase)));
        }

        public static bool EqualsToOneOf(this string value, params string[] compValues)
        {
            for (int i=0; i<compValues.Length; i++)
            {
                if (value.Equals(compValues[i]))
                    return true;
            }
            return false;
        }

        public static int IndexOfFirst(this string text, int startIndex, params string[] values)
        {
            int pos = -1;
            for (int i=0; i<values.Length; i++)
            {
                int partPos = text.IndexOf(values[i], startIndex);
                if ((partPos >= 0) && ((pos < 0) || (pos > partPos)))
                    pos = partPos;
            }
            return pos;
        }

    }
}
