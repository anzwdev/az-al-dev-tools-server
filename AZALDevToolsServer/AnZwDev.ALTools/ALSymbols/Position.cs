using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class Position
    {

        public int line { get; set; }
        public int character { get; set; }

        public Position() : this(0, 0)
        {
        }

        public Position(int newLine, int newCharacter)
        {
            this.line = newLine;
            this.character = newCharacter;
        }

    }
}
