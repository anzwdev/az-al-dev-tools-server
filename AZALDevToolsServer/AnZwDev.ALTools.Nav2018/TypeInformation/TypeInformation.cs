/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.TypeInformation
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
