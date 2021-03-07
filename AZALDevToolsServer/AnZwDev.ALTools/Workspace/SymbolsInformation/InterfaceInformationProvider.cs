using AnZwDev.ALTools.ALSymbols.ALAppPackages;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class InterfaceInformationProvider
    {

        public List<InterfaceInformation> GetInterfaces(ALProject project)
        {
            List<InterfaceInformation> infoList = new List<InterfaceInformation>();
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                if (dependency.Symbols != null)
                    AddInterfaces(infoList, dependency.Symbols);
            }
            if (project.Symbols != null)
                AddInterfaces(infoList, project.Symbols);
            return infoList;
        }

        private void AddInterfaces(List<InterfaceInformation> infoList, ALAppSymbolReference symbols)
        {
            if (symbols.Interfaces != null)
            {
                for (int i = 0; i < symbols.Interfaces.Count; i++)
                    infoList.Add(new InterfaceInformation(symbols.Interfaces[i]));
            }
        }

    }
}
