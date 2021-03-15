using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ReportInformation: SymbolWithIdInformation
    {

        public ReportInformation()
        {
        }

        public ReportInformation(ALAppReport symbol): base(symbol)
        {
        }

    }
}
