using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALSyntaxHelper
    {

        public static bool NameNeedsEcoding(string name)
        {
            if (!String.IsNullOrWhiteSpace(name))
            {
                for (int i = 0; i < name.Length; i++)
                {
                    char nameChar = name[i];
                    if (!(
                        ((nameChar >= 'a') && (nameChar <= 'z')) ||
                        ((nameChar >= 'A') && (nameChar <= 'Z')) ||
                        ((nameChar >= '0') && (nameChar <= '9')) ||
                        (nameChar == '_')))
                        return true;
                }
            }
            return false;
        }

        public static string DecodeName(string name)
        {
            if (name != null)
            {
                name = name.Trim();
                if (name.StartsWith("\""))
                {
                    name = name.Substring(1);
                    if (name.EndsWith("\""))
                        name = name.Substring(0, name.Length - 1);
                    name = name.Replace("\"\"", "\"");
                }
            }
            return name;
        }

        public static string EncodeName(string name)
        {
            if (NameNeedsEcoding(name))
                return "\"" + name.Replace("\"", "\"\"") + "\"";
            return name;
        }

    }
}
