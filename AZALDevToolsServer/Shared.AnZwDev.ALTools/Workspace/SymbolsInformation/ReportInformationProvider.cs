using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ReportInformationProvider
    {

        public ReportInformationProvider()
        {
        }

        #region Get list of reports

        public List<ReportInformation> GetReports(ALProject project)
        {
            List<ReportInformation> infoList = new List<ReportInformation>();
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                if (dependency.Symbols != null)
                    AddReports(infoList, dependency.Symbols);
            }
            if (project.Symbols != null)
                AddReports(infoList, project.Symbols);
            return infoList;
        }

        private void AddReports(List<ReportInformation> infoList, ALAppSymbolReference symbols)
        {
            if (symbols.Reports != null)
            {
                for (int i = 0; i < symbols.Reports.Count; i++)
                    infoList.Add(new ReportInformation(symbols.Reports[i]));
            }
        }

        #endregion

        #region Find report

        protected ALAppReport FindReport(ALProject project, string name)
        {
            ALAppReport report = FindReport(project.Symbols, name);
            if (report != null)
                return report;
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                report = FindReport(dependency.Symbols, name);
                if (report != null)
                    return report;
            }
            return null;
        }

        protected ALAppReport FindReport(ALProject project, int id)
        {
            ALAppReport report = FindReport(project.Symbols, id);
            if (report != null)
                return report;
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                report = FindReport(dependency.Symbols, id);
                if (report != null)
                    return report;
            }
            return null;
        }

        protected ALAppReport FindReport(ALAppSymbolReference symbols, string name)
        {
            if ((symbols != null) && (symbols.Reports != null))
                return symbols.Reports
                    .Where(p => (name.Equals(p.Name, StringComparison.CurrentCultureIgnoreCase)))
                    .FirstOrDefault();
            return null;
        }

        protected ALAppReport FindReport(ALAppSymbolReference symbols, int id)
        {
            if ((symbols != null) && (symbols.Reports != null))
                return symbols.Reports
                    .Where(p => (p.Id == id))
                    .FirstOrDefault();
            return null;
        }

        #endregion

        #region Find report extension

        protected ALAppReportExtension FindReportExtension(ALAppSymbolReference symbols, string reportName)
        {
            if ((symbols != null) && (symbols.ReportExtensions != null))
                return symbols.ReportExtensions
                    .Where(p => (reportName.Equals(p.Target, StringComparison.CurrentCultureIgnoreCase)))
                    .FirstOrDefault();
            return null;
        }

        #endregion

        #region Find report data item

        protected ALAppReportDataItem FindReportDataItem(ALProject project, string reportName, string dataItemName)
        {
            ALAppReportDataItem dataItem;
            //find report data item
            ALAppReport report = this.FindReport(project, reportName);
            if ((report != null) && (report.DataItems != null))
            {
                dataItem = report.FindDataItem(dataItemName);
                if (dataItem != null)
                    return dataItem;
            }

            //find report extension data item
            ALAppReportExtension reportExtension = FindReportExtension(project.Symbols, reportName);
            if ((reportExtension != null) && (reportExtension.DataItems != null))
            {
                dataItem = reportExtension.FindDataItem(dataItemName);
                if (dataItem != null)
                    return dataItem;
            }

            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                reportExtension = FindReportExtension(dependency.Symbols, reportName);
                if ((reportExtension != null) && (reportExtension.DataItems != null))
                {
                    dataItem = reportExtension.FindDataItem(dataItemName);
                    if (dataItem != null)
                        return dataItem;
                }
            }
            return null;
        }

        #endregion

        #region Get report data item details

        public ReportDataItemInformation GetReportDataItemInformationDetails(ALProject project, string reportName, string dataItemName, bool getExistingFields, bool getAvailableFields)
        {
            ALAppReportDataItem reportDataItem = this.FindReportDataItem(project, reportName, dataItemName);
            if (reportDataItem == null)
                return null;

            ReportDataItemInformation reportDataItemInformation = new ReportDataItemInformation(reportDataItem);
            if ((!String.IsNullOrWhiteSpace(reportDataItem.RelatedTable)) && (getExistingFields || getAvailableFields))
            {
                TableInformationProvider tableInformationProvider = new TableInformationProvider();
                List<TableFieldInformaton> allTableFieldsList = tableInformationProvider.GetTableFields(project, reportDataItem.RelatedTable, false, false);

                Dictionary<string, TableFieldInformaton> availableTableFieldsDict = allTableFieldsList.ToDictionary();
                List<TableFieldInformaton> reportDataItemFields = new List<TableFieldInformaton>();

                if (reportDataItem.Columns != null)
                    this.CollectReportDataItemFields(reportDataItemInformation.Name, reportDataItem.Columns, availableTableFieldsDict, reportDataItemFields);

                //collect fields from report extensions
                foreach (ALProjectDependency dependency in project.Dependencies)
                {
                    ALAppReportExtension reportExtension = FindReportExtension(dependency.Symbols, reportName);
                    if ((reportExtension != null) && (reportExtension.Columns != null))
                        this.CollectReportDataItemFields(reportDataItemInformation.Name, reportExtension.Columns.Where(p => (dataItemName.Equals(p.OwningDataItemName, StringComparison.CurrentCultureIgnoreCase))), availableTableFieldsDict, reportDataItemFields);
                }

                //add fields
                if (getExistingFields)
                    reportDataItemInformation.ExistingTableFields = reportDataItemFields;
                if (getAvailableFields)
                    reportDataItemInformation.AvailableTableFields = availableTableFieldsDict.Values.ToList();
            }

            return reportDataItemInformation;
        }

        protected void CollectReportDataItemFields(string dataItemName, IEnumerable<ALAppReportColumn> columnsList, Dictionary<string, TableFieldInformaton> availableTableFieldsDict, List<TableFieldInformaton> reportDataItemFields)
        {
            foreach (ALAppReportColumn reportColumn in columnsList)
            {
                if (!String.IsNullOrWhiteSpace(reportColumn.SourceExpression))
                {
                    ALMemberAccessExpression memberAccessExpression = ALSyntaxHelper.DecodeMemberAccessExpression(reportColumn.SourceExpression);
                    bool isMemberAccess = !String.IsNullOrWhiteSpace(memberAccessExpression.Expression);

                    string sourceExpression = null;
                    if (!isMemberAccess)
                        sourceExpression = memberAccessExpression.Name.ToLower();
                    else if (dataItemName.Equals(memberAccessExpression.Name, StringComparison.CurrentCultureIgnoreCase))
                        sourceExpression = memberAccessExpression.Expression.ToLower();

                    if ((!String.IsNullOrWhiteSpace(sourceExpression)) && (availableTableFieldsDict.ContainsKey(sourceExpression)))
                    {
                        reportDataItemFields.Add(availableTableFieldsDict[sourceExpression]);
                        availableTableFieldsDict.Remove(sourceExpression);
                    }
                }
            }
        }

        #endregion

    }
}
