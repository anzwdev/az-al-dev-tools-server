using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Helpers
{
    public static class StringHelper
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

    }
}
