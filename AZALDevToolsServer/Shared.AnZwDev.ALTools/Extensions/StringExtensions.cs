﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Extensions
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

        public static string RemovePrefixSuffix(this string text, List<string> prefixes, List<string> suffixes, List<string> affixes)
        {
            if (text != null)
            {
                bool found = false;
                //remove first suffix
                text = text.RemoveSuffix(suffixes, out found);
                if (found)
                    return text;

                //remove first prefix
                text = text.RemovePrefix(prefixes, out found);
                if (found)
                    return text;

                //remove first prefix/suffix
                text = text.RemoveSuffix(affixes, out found);
                if (found)
                    return text;
                text = text.RemovePrefix(affixes, out found);
                if (found)
                    return text;
            }
            return text;
        }

        public static string RemovePrefix(this string text, List<string> prefixes, out bool found)
        {
            found = false;
            if (prefixes != null)
            {
                for (int i = 0; i < prefixes.Count; i++)
                {
                    if ((!String.IsNullOrWhiteSpace(prefixes[i])) && (text.StartsWith(prefixes[i])))
                    {
                        found = true;
                        return text.Substring(prefixes[i].Length).Trim();
                    }
                }
            }
            return text;
        }

        public static string RemoveSuffix(this string text, List<string> suffixes, out bool found)
        {
            found = false;
            if (suffixes != null)
            {
                for (int i = 0; i < suffixes.Count; i++)
                {
                    if ((!String.IsNullOrWhiteSpace(suffixes[i])) && (text.EndsWith(suffixes[i])))
                    {
                        found = true;
                        return text.Substring(0, text.Length - suffixes[i].Length).Trim();
                    }
                }
            }
            return text;
        }


        public static List<string> ToSingleElementList(this string text)
        {
            List<string> list = new List<string>();
            list.Add(text);
            return list;
        }

    }
}
