using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.ALAppPackages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            if ((symbols != null) && (symbols.TableExtensions != null))
                return symbols.PageExtensions
                    .Where(p => (pageName.Equals(p.TargetObject, StringComparison.CurrentCultureIgnoreCase)))
                    .FirstOrDefault();
            return null;
        }

        #endregion

        #region Get page details

        public PageInformation GetPageDetails(ALProject project, string pageName, bool getPageFields, bool getAvailableFields)
        {
            ALAppPage pageSymbol = this.FindPage(project, pageName);
            if (pageSymbol == null)
                return null;
            PageInformation pageInformation = new PageInformation(pageSymbol);

            //collect fields
            if ((!String.IsNullOrWhiteSpace(pageInformation.Source)) && (getPageFields || getAvailableFields))
            {
                TableInformationProvider tableInformationProvider = new TableInformationProvider();
                List<TableFieldInformaton> allTableFieldsList = tableInformationProvider.GetTableFields(project, pageInformation.Source);

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

                if (getPageFields)
                    pageInformation.PageTableFields = pageTableFields;
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

    }
}
