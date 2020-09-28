using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.TypeInformation
{
    public class ProjectTypesInformation
    {
        protected Dictionary<string, TableTypeInformation> Tables { get; }

        public ProjectTypesInformation()
        {
            this.Tables = new Dictionary<string, TableTypeInformation>();
        }

        public void Clear()
        {
            this.Tables.Clear();
        }

        public void Add(TableTypeInformation table)
        {
            if (!this.Tables.ContainsKey(table.Name.ToLower()))
                this.Tables.Add(table.Name.ToLower(), table);
        }

        public TableTypeInformation GetTable(string name)
        {
            name = name.ToLower();
            if (this.Tables.ContainsKey(name))
                return this.Tables[name];
            return null;
        }

        public IEnumerable<TableTypeInformation> GetAllTables()
        {
            return this.Tables.Values;
        }

    }
}
