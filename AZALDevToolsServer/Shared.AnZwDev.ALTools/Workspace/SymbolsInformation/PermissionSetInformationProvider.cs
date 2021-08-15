using AnZwDev.ALTools.ALSymbolReferences;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class PermissionSetInformationProvider
    {

        public List<PermissionSetInformation> GetPermissionSets(ALProject project, bool includeNonAccessible)
        {
            List<PermissionSetInformation> infoList = new List<PermissionSetInformation>();
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                if (dependency.Symbols != null)
                    AddPermissionSets(infoList, dependency.Symbols, includeNonAccessible || dependency.InternalsVisible);
            }
            if (project.Symbols != null)
                AddPermissionSets(infoList, project.Symbols, true);
            return infoList;
        }

        private void AddPermissionSets(List<PermissionSetInformation> infoList, ALAppSymbolReference symbols, bool includeInternal)
        {
            if (symbols.PermissionSets != null)
            {
                for (int i = 0; i < symbols.PermissionSets.Count; i++)
                {
                    if ((includeInternal) || (symbols.PermissionSets[i].GetAccessMode() != ALAppAccessMode.Internal))
                        infoList.Add(new PermissionSetInformation(symbols.PermissionSets[i]));
                }
            }
        }
         
    }
}
