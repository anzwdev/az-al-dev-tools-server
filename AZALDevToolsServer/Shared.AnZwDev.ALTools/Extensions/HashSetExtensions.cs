using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class HashSetExtensions
    {

        public static HashSet<string> Create(params string[] values)
        {
            return new HashSet<string>(values);
        }

    }
}
