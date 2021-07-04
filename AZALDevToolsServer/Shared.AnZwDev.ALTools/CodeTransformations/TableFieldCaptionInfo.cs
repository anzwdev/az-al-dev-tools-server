using AnZwDev.ALTools.Workspace.SymbolsInformation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class TableFieldCaptionInfo
    {

        public LabelInformation Caption { get; set; }
        public string Description { get; set; }

        public TableFieldCaptionInfo() : this(null, null)
        {
        }

        public TableFieldCaptionInfo(LabelInformation caption) : this(caption, null)
        {
        }

        public TableFieldCaptionInfo(LabelInformation caption, string description)
        {
            this.Caption = caption;
            this.Description = description;
        }

    }
}
