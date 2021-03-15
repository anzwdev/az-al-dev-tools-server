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

        public TableInformation(ALAppTable table)
        {
            this.Id = table.Id;
            this.Name = table.Name;
            if (table.Properties != null)
                this.Caption = table.Properties.GetValue("Caption");
        }


    }
}
