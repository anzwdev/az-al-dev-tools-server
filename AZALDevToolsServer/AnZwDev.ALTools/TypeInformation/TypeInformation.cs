using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.TypeInformation
{
    public class TypeInformation
    {

        public string Name { get; set; }

        public TypeInformation(): this(null)
        {
        }

        public TypeInformation(string name)
        {
            this.Name = name;
        }

    }
}
