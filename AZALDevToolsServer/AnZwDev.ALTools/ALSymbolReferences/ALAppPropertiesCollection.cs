using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppPropertiesCollection : List<ALAppProperty>
    {

        public ALAppPropertiesCollection()
        {
        }

        public string GetValue(string name)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (name.Equals(this[i].Name, StringComparison.CurrentCultureIgnoreCase))
                    return this[i].Value;
            }
            return null;
        }

    }
}
