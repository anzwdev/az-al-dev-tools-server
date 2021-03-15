using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class TableInformationProvider
    {

        #region Get list of tables

        public List<TableInformation> GetTables(ALProject project)
        {
            List<TableInformation> infoList = new List<TableInformation>();
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                if (dependency.Symbols != null)
                    AddTables(infoList, dependency.Symbols);
            }
            if (project.Symbols != null)
                AddTables(infoList, project.Symbols);
            return infoList;
        }

        private void AddTables(List<TableInformation> infoList, ALAppSymbolReference symbols)
        {
            if (symbols.Tables != null)
            {
                for (int i = 0; i < symbols.Tables.Count; i++)
                    infoList.Add(new TableInformation(symbols.Tables[i]));
            }
        }

        #endregion

        #region Find table

        protected ALAppTable FindTable(ALProject project, string name)
        {
            ALAppTable table = FindTable(project.Symbols, name);
            if (table != null)
                return table;
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                table = FindTable(dependency.Symbols, name);
                if (table != null)
                    return table;
            }
            return null;
        }

        protected ALAppTable FindTable(ALProject project, int id)
        {
            ALAppTable table = FindTable(project.Symbols, id);
            if (table != null)
                return table;
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                table = FindTable(dependency.Symbols, id);
                if (table != null)
                    return table;
            }
            return null;
        }

        protected ALAppTable FindTable(ALAppSymbolReference symbols, string name)
        {
            if ((symbols != null) && (symbols.Tables != null))
                return symbols.Tables
                    .Where(p => (name.Equals(p.Name, StringComparison.CurrentCultureIgnoreCase)))
                    .FirstOrDefault();
            return null;
        }

        protected ALAppTable FindTable(ALAppSymbolReference symbols, int id)
        {
            if ((symbols != null) && (symbols.Tables != null))
                return symbols.Tables
                    .Where(p => (p.Id == id))
                    .FirstOrDefault();
            return null;
        }

        #endregion

        #region Find table extension

        protected ALAppTableExtension FindTableExtension(ALAppSymbolReference symbols, string tableName)
        {
            if ((symbols != null) && (symbols.TableExtensions != null))
                return symbols.TableExtensions
                    .Where(p => (tableName.Equals(p.TargetObject, StringComparison.CurrentCultureIgnoreCase)))
                    .FirstOrDefault();
            return null;
        }

        #endregion

        #region Get table fields

        public List<TableFieldInformaton> GetTableFields(ALProject project, string tableName)
        {
            List<TableFieldInformaton> fields = new List<TableFieldInformaton>();

            //find table
            ALAppTable table = this.FindTable(project, tableName);
            if (table == null)
                return fields;

            //add fields from table extensions
            this.AddFields(fields, table.Fields);

            //add table extension fields
            ALAppTableExtension tableExtension;
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                tableExtension = FindTableExtension(dependency.Symbols, tableName);
                if (tableExtension != null)
                    this.AddFields(fields, tableExtension.Fields);
            }

            //add virtual system fields
            this.AddSystemFields(fields);

            return fields;
        }

        protected void AddFields(List<TableFieldInformaton> fields, ALAppElementsCollection<ALAppTableField> fieldReferencesList)
        {
            if (fieldReferencesList != null)
            {
                foreach (ALAppTableField fieldReference in fieldReferencesList)
                {
                    fields.Add(new TableFieldInformaton(fieldReference));
                }
            }
        }
        
        protected void AddSystemFields(List<TableFieldInformaton> fields)
        {
            fields.Add(new TableFieldInformaton(2000000000, "SystemId", "Guid"));
            fields.Add(new TableFieldInformaton(2000000000, "SystemCreatedAt", "DateTime"));
            fields.Add(new TableFieldInformaton(2000000000, "SystemCreatedBy", "Guid"));
            fields.Add(new TableFieldInformaton(2000000000, "SystemModifiedAt", "DateTime"));
            fields.Add(new TableFieldInformaton(2000000000, "SystemModifiedBy", "Guid"));
        }

        #endregion

    }
}
