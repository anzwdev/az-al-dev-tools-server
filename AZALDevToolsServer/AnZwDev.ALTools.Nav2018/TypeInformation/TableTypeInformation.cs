/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.TypeInformation
{
    public class TableTypeInformation : TypeInformation
    {

        protected Dictionary<string, TableFieldTypeInformation> Fields { get; }

        public TableTypeInformation(): this((string)null)
        {
        }

        public TableTypeInformation(string name): base(name)
        {
            this.Fields = new Dictionary<string, TableFieldTypeInformation>();
        }

        public TableTypeInformation(TableSyntax tableSyntax) : this(tableSyntax.GetNameStringValue())
        {
            FieldListSyntax fields = tableSyntax.Fields;
            if (fields != null)
            {
                SyntaxList<FieldSyntax> fieldsList = fields.Fields;
                if (fieldsList != null)
                {
                    foreach (FieldSyntax fieldSyntax in fieldsList)
                    {
                        TableFieldTypeInformation fieldTypeInformation = new TableFieldTypeInformation(fieldSyntax);
                        Add(fieldTypeInformation);
                    }
                }
            }
        }

        public void Add(TableFieldTypeInformation field)
        {
            string name = field.Name.ToLower();
            if (!this.Fields.ContainsKey(name))
                this.Fields.Add(name, field);
        }

        public TableFieldTypeInformation GetField(string name)
        {
            name = name.ToLower();
            if (this.Fields.ContainsKey(name))
                return this.Fields[name];
            return null;
        }

        public IEnumerable<TableFieldTypeInformation> GetAllFields()
        {
            return this.Fields.Values;
        }

    }
}
