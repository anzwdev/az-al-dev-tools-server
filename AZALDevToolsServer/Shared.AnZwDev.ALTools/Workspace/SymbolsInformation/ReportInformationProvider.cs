using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;

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

        protected ALAppReport ALAppReport(ALProject project, int id)
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

        #region Get report details



        #endregion

        #region Get report data item details


        
        #endregion

    }
}
