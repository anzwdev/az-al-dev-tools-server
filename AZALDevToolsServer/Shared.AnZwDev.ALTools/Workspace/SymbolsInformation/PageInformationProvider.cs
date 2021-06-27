﻿using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class PageInformationProvider
    {

        public PageInformationProvider()
        {
        }

        #region Get list of pages

        public List<PageInformation> GetPages(ALProject project)
        {
            List<PageInformation> infoList = new List<PageInformation>();
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                if (dependency.Symbols != null)
                    AddPages(infoList, dependency.Symbols);
            }
            if (project.Symbols != null)
                AddPages(infoList, project.Symbols);
            return infoList;
        }

        private void AddPages(List<PageInformation> infoList, ALAppSymbolReference symbols)
        {
            if (symbols.Pages != null)
            {
                for (int i = 0; i < symbols.Pages.Count; i++)
                    infoList.Add(new PageInformation(symbols.Pages[i]));
            }
        }

        #endregion

        #region Find page

        protected ALAppPage FindPage(ALProject project, string name)
        {
            ALAppPage page = FindPage(project.Symbols, name);
            if (page != null)
                return page;
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                page = FindPage(dependency.Symbols, name);
                if (page != null)
                    return page;
            }
            return null;
        }

        protected ALAppPage FindPage(ALProject project, int id)
        {
            ALAppPage page = FindPage(project.Symbols, id);
            if (page != null)
                return page;
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                page = FindPage(dependency.Symbols, id);
                if (page != null)
                    return page;
            }
            return null;
        }

        protected ALAppPage FindPage(ALAppSymbolReference symbols, string name)
        {
            if ((symbols != null) && (symbols.Pages != null))
                return symbols.Pages
                    .Where(p => (name.Equals(p.Name, StringComparison.CurrentCultureIgnoreCase)))
                    .FirstOrDefault();
            return null;
        }

        protected ALAppPage FindPage(ALAppSymbolReference symbols, int id)
        {
            if ((symbols != null) && (symbols.Pages != null))
                return symbols.Pages
                    .Where(p => (p.Id == id))
                    .FirstOrDefault();
            return null;
        }

        #endregion

        #region Find page extension

        protected ALAppPageExtension FindPageExtension(ALAppSymbolReference symbols, string pageName)
        {
            if ((symbols != null) && (symbols.PageExtensions != null))
                return symbols.PageExtensions
                    .Where(p => (pageName.Equals(p.TargetObject, StringComparison.CurrentCultureIgnoreCase)))
                    .FirstOrDefault();
            return null;
        }

        #endregion

        #region Find pages for table

        protected List<ALAppPage> FindPagesForTable(ALProject project, string tableName)
        {
            List<ALAppPage> pagesList = new List<ALAppPage>();
            this.FindPagesForTable(project.Symbols, tableName, pagesList);
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                this.FindPagesForTable(dependency.Symbols, tableName, pagesList);
            }
            return pagesList;
        }

        protected void FindPagesForTable(ALAppSymbolReference symbols, string tableName, List<ALAppPage> pagesList)
        {
            if ((symbols != null) && (symbols.Pages != null))
            {
                foreach (ALAppPage page in symbols.Pages)
                {
                    string pageSourceTable = page.GetSourceTable();
                    if ((pageSourceTable != null) && (pageSourceTable.Equals(tableName, StringComparison.CurrentCultureIgnoreCase)))
                        pagesList.Add(page);
                }
            }
        }

        #endregion

        #region Find page extesnions for pages

        protected List<ALAppPageExtension> FindPageExtensionsForPages(ALProject project, List<ALAppPage> pagesList)
        {
            List<ALAppPageExtension> pageExtensionsList = new List<ALAppPageExtension>();
            if ((pagesList != null) && (pagesList.Count > 0))
            {
                //collect page names
                HashSet<string> pageNamesSet = new HashSet<string>();
                foreach (ALAppPage page in pagesList)
                {
                    if (page.Name != null)
                        pageNamesSet.Add(page.Name.ToLower());
                }

                //find page extensions
                if (project.Symbols != null)
                    this.FindPageExtensionsForPages(project.Symbols, pageNamesSet, pageExtensionsList);

                foreach (ALProjectDependency dependency in project.Dependencies)
                {
                    this.FindPageExtensionsForPages(dependency.Symbols, pageNamesSet, pageExtensionsList);
                }
            }
            return pageExtensionsList;
        }

        protected void FindPageExtensionsForPages(ALAppSymbolReference symbols, HashSet<string> pageNamesSet, List<ALAppPageExtension> pageExtensionsList)
        {
            if ((symbols != null) && (symbols.PageExtensions != null))
            {
                foreach (ALAppPageExtension pageExtension in symbols.PageExtensions)
                {
                    if (pageExtension.TargetObject != null)
                    {
                        string pageName = pageExtension.TargetObject.ToLower();
                        if (pageNamesSet.Contains(pageName))
                            pageExtensionsList.Add(pageExtension);
                    }
                }
            }
        }


        #endregion

        #region Get page details

        public PageInformation GetPageDetails(ALProject project, string pageName, bool getExistingFields, bool getAvailableFields)
        {
            ALAppPage pageSymbol = this.FindPage(project, pageName);
            if (pageSymbol == null)
                return null;
            PageInformation pageInformation = new PageInformation(pageSymbol);

            //collect fields
            if ((!String.IsNullOrWhiteSpace(pageInformation.Source)) && (getExistingFields || getAvailableFields))
            {
                TableInformationProvider tableInformationProvider = new TableInformationProvider();
                List<TableFieldInformaton> allTableFieldsList = tableInformationProvider.GetTableFields(project, pageInformation.Source, false, false);

                Dictionary<string, TableFieldInformaton> availableTableFieldsDict = allTableFieldsList.ToDictionary();

                List<TableFieldInformaton> pageTableFields = new List<TableFieldInformaton>();
                
                if (pageSymbol.Controls != null)
                    this.CollectVisibleFields(pageSymbol.Controls, availableTableFieldsDict, pageTableFields);

                //collect fields from page extensions
                foreach (ALProjectDependency dependency in project.Dependencies)
                {
                    ALAppPageExtension pageExtensionSymbol = this.FindPageExtension(dependency.Symbols, pageName);
                    if ((pageExtensionSymbol != null) && (pageExtensionSymbol.ControlChanges != null))
                        this.CollectVisibleFields(pageExtensionSymbol.ControlChanges, availableTableFieldsDict, pageTableFields);
                }

                if (getExistingFields)
                    pageInformation.ExistingTableFields = pageTableFields;
                if (getAvailableFields)
                    pageInformation.AvailableTableFields = availableTableFieldsDict.Values.ToList();
            }

            return pageInformation;
        }

        protected void CollectVisibleFields(ALAppElementsCollection<ALAppPageControlChange> controlsList, Dictionary<string, TableFieldInformaton> availableTableFieldsDict, List<TableFieldInformaton> pageTableFields)
        {
            for (int i=0; i<controlsList.Count; i++)
            {
                if (controlsList[i].Controls != null)
                    this.CollectVisibleFields(controlsList[i].Controls, availableTableFieldsDict, pageTableFields);
            }
        }


        protected void CollectVisibleFields(ALAppElementsCollection<ALAppPageControl> controlsList, Dictionary<string, TableFieldInformaton> availableTableFieldsDict, List<TableFieldInformaton> pageTableFields)
        {
            for (int i=0; i<controlsList.Count; i++)
            {
                ALAppPageControl pageControl = controlsList[i];
                if (!String.IsNullOrWhiteSpace(pageControl.Expression))
                {
                    string sourceExpression = pageControl.Expression;
                    if (!String.IsNullOrWhiteSpace(sourceExpression))
                    {
                        sourceExpression = sourceExpression.Trim();
                        if (sourceExpression.StartsWith("Rec.", StringComparison.CurrentCultureIgnoreCase))
                            sourceExpression = sourceExpression.Substring(4).Trim();
                        sourceExpression = ALSyntaxHelper.DecodeName(sourceExpression).ToLower();
                        if (availableTableFieldsDict.ContainsKey(sourceExpression))
                        {
                            pageTableFields.Add(availableTableFieldsDict[sourceExpression]);
                            availableTableFieldsDict.Remove(sourceExpression);
                        }
                    }
                }

                if (pageControl.Controls != null)
                    this.CollectVisibleFields(pageControl.Controls, availableTableFieldsDict, pageTableFields);
            }
        }

        #endregion

        #region Page fields

        public List<TableFieldInformaton> GetAllTableFieldsForPage(ALProject project, string pageName, bool includeDisabled, bool includeObsolete)
        {
            ALAppPage page = this.FindPage(project, pageName);
            if (page != null)
            {
                string tableName = page.Properties.GetValue("SourceTable");
                if (!String.IsNullOrWhiteSpace(tableName))
                {
                    TableInformationProvider tableInformationProvider = new TableInformationProvider();
                    return tableInformationProvider.GetTableFields(project, tableName, includeDisabled, includeObsolete);
                }
            }
            return null;
        }

        #endregion

        #region Page variables

        public List<ALAppVariable> GetPageVariables(ALProject project, string pageName)
        {
            List<ALAppVariable> variables = new List<ALAppVariable>();
            
            ALAppPage page = this.FindPage(project, pageName);
            if ((page != null) && (page.Variables != null) && (page.Variables.Count > 0))
                variables.AddRange(page.Variables);

            ALAppPageExtension pageExtension = FindPageExtension(project.Symbols, pageName);
            if ((pageExtension != null) && (pageExtension.Variables != null) && (pageExtension.Variables.Count > 0))
                variables.AddRange(pageExtension.Variables);
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                pageExtension = FindPageExtension(dependency.Symbols, pageName);
                if ((pageExtension != null) && (pageExtension.Variables != null) && (pageExtension.Variables.Count > 0))
                    variables.AddRange(pageExtension.Variables);
            }

            return variables;
        }

        #endregion

        #region Tooltips information

        public void CollectToolTips(ALProject project, string tableName, List<TableFieldInformaton> tableFieldsList)
        {
            //find pages and page extensions
            List<ALAppPage> pagesList = this.FindPagesForTable(project, tableName);
            List<ALAppPageExtension> pageExtensionsList = this.FindPageExtensionsForPages(project, pagesList);
            
            //collect fields
            Dictionary<string, TableFieldInformaton> tableFieldsDictionary = new Dictionary<string, TableFieldInformaton>();
            foreach (TableFieldInformaton tableField in tableFieldsList)
            {
                if (!String.IsNullOrWhiteSpace(tableField.Name))
                {
                    string tableFieldName = tableField.Name.ToLower();
                    if (!tableFieldsDictionary.ContainsKey(tableFieldName))
                        tableFieldsDictionary.Add(tableFieldName, tableField);
                }
            }
            
            //process pages
            foreach (ALAppPage page in pagesList)
            {
                this.CollectToolTips(page.Controls, tableFieldsDictionary);
            }

            //process page extensions
            foreach (ALAppPageExtension pageExtension in pageExtensionsList)
            {
                this.CollectToolTips(pageExtension.ControlChanges, tableFieldsDictionary);
            }
        }

        protected void CollectToolTips(List<ALAppPageControlChange> controlChangesList, Dictionary<string, TableFieldInformaton> tableFields)
        {
            if (controlChangesList != null)
            {
                foreach (ALAppPageControlChange controlChange in controlChangesList)
                {
                    this.CollectToolTips(controlChange.Controls, tableFields);
                }
            }
        }

        protected void CollectToolTips(List<ALAppPageControl> controlsList, Dictionary<string, TableFieldInformaton> tableFields)
        {
            if (controlsList != null)
            {
                foreach (ALAppPageControl control in controlsList)
                {
                    //check field and tooltip value
                    if (!String.IsNullOrWhiteSpace(control.Expression))
                    {
                        string toolTip = control.Properties.GetValue("ToolTip");
                        if (!String.IsNullOrWhiteSpace(toolTip))
                        {
                            string fieldName = null;
                            ALMemberAccessExpression memberAccessExpression = ALSyntaxHelper.DecodeMemberAccessExpression(control.Expression);
                            if (String.IsNullOrWhiteSpace(memberAccessExpression.Expression))
                                fieldName = memberAccessExpression.Name;
                            else if (memberAccessExpression.Expression.Equals("rec", StringComparison.CurrentCultureIgnoreCase))
                                fieldName = memberAccessExpression.Expression;
                            if (!String.IsNullOrWhiteSpace(fieldName))
                            {
                                fieldName.ToLower();
                                if (tableFields.ContainsKey(fieldName))
                                {
                                    TableFieldInformaton tableFieldInformaton = tableFields[fieldName];
                                    if (tableFieldInformaton.ToolTips == null)
                                        tableFieldInformaton.ToolTips = new List<string>();
                                    if (!tableFieldInformaton.ToolTips.Contains(toolTip))
                                        tableFieldInformaton.ToolTips.Add(toolTip);
                                }
                            }
                        }
                    }
                    //process child controls
                    if (control.Controls != null)
                        this.CollectToolTips(control.Controls, tableFields);
                }
            }
        }

        #endregion

    }
}

