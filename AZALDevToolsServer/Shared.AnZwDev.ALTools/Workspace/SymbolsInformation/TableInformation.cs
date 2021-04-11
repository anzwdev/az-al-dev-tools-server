using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class TableInformation : SymbolWithIdInformation
    {

        public TableInformation()
        {
        }

        public TableInformation(ALAppTable table): base(table)
        {
            if (table.Properties != null)
                this.Caption = table.Properties.GetValue("Caption");
        }


    }
}
