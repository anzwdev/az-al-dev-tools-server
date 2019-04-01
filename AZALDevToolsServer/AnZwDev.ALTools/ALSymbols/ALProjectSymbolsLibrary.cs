using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALProjectSymbolsLibrary : ALSymbolsLibrary
    {

        public string Path { get; set; }

        public ALProjectSymbolsLibrary(string projectPath)
        {
            this.Path = projectPath;
        }

        public override bool Load(bool forceReload)
        {
            return base.Load(forceReload);
        }

    }
}
