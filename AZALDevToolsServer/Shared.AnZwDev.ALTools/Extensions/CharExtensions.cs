using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class CharExtensions
    {

        public static bool IsNewLine(this char character)
        {
            return (character == '\n') || (character == '\r');
        }

    }
}
