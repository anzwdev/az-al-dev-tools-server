using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.TypeInformation
{
    public class TableFieldTypeInformation: TypeInformation
    {

        public int Id { get; set; }
        public string Caption { get; set; }
        public string DataType { get; set; }

        public TableFieldTypeInformation(): this((string)null)
        {
        }

        public TableFieldTypeInformation(string name): base(name)
        {
            this.Caption = null;
            this.DataType = null;
        }

        public TableFieldTypeInformation(FieldSyntax fieldSyntax): this(fieldSyntax.GetNameStringValue())
        {
            PropertyValueSyntax propValue = fieldSyntax.GetPropertyValue("Caption");
            if (propValue != null)
                this.Caption = ALSyntaxHelper.DecodeString(propValue.ToString());
            this.DataType = fieldSyntax.Type.ToString();
        }

    }
}
