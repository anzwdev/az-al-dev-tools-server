using AnZwDev.ALTools.ALSymbols.ALAppPackages;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class CodeunitInformationProvider
    {

        public List<CodeunitInformation> GetCodeunits(ALProject project)
        {
            List<CodeunitInformation> infoList = new List<CodeunitInformation>();
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                if (dependency.Symbols != null)
                    AddCodeunits(infoList, dependency.Symbols);
            }
            if (project.Symbols != null)
                AddCodeunits(infoList, project.Symbols);
            return infoList;
        }

        private void AddCodeunits(List<CodeunitInformation> infoList, ALAppSymbolReference symbols)
        {
            if (symbols.Codeunits != null)
            {
                for (int i = 0; i < symbols.Codeunits.Count; i++)
                    infoList.Add(new CodeunitInformation(symbols.Codeunits[i]));
            }
        }


    }
}
