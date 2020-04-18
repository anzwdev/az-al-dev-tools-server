using AnZwDev.ALTools.ALProxy;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALFullSyntaxTree
    {

        public ALExtensionProxy ALExtensionProxy { get; }
        public ALFullSyntaxTreeNode Root { get; set; }

        public ALFullSyntaxTree(ALExtensionProxy alExtensionProxy)
        {
            this.ALExtensionProxy = alExtensionProxy;
        }

        public void Load(string source, string filePath)
        {
            ALFullSyntaxTreeReader reader = new ALFullSyntaxTreeReader(this.ALExtensionProxy);
            if (!String.IsNullOrEmpty(source))
                this.Root = reader.ProcessSourceCode(source);
            else
                this.Root = reader.ProcessSourceFile(filePath);
                     

        }


    }
}
