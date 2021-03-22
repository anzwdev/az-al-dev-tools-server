using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class DictionaryExtensions
    {

        public static string GetStringValue(this Dictionary<string, string> dictionary, string key, string defaultValue = null)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            return defaultValue;
        }

        public static bool GetBoolValue(this Dictionary<string, string> dictionary, string key, bool defaultValue = false)
        {
            string value = dictionary.GetStringValue(key);
            if (String.IsNullOrWhiteSpace(value))
                return defaultValue;
            return ((value.ToLower() == "true") || (value == "1"));
        }

        public static string[] GetStringArrayValue(this Dictionary<string, string> dictionary, string key)
        {
            string value = dictionary.GetStringValue(key);
            if (String.IsNullOrWhiteSpace(value))
                return null;
            char[] split = { ',' };
            return value.Split(split);
        }

    }
}
