using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALDocumentSymbolsLibrary : ALSymbolsLibrary
    {

        public string Path { get; set; }
        public string Content { get; set; }

        public ALDocumentSymbolsLibrary(string filePath)
        {
            this.Path = filePath;
            this.Content = null;
        }

        public void DocumentOpened(string content)
        {
            this.Content = content;
        }

        public void DocumentClosed()
        {
            this.Content = null;
        }

        public void DocumentChanged(string content)
        {
            this.Content = content;
        }

    }
}
