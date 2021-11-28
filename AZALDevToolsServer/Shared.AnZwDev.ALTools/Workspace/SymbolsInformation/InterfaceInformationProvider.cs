using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.MergedReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class InterfaceInformationProvider : BaseObjectInformationProvider<ALAppInterface>
    {

        protected override MergedALAppObjectsCollection<ALAppInterface> GetALAppObjectsCollection(ALProject project)
        {
            return project.AllSymbols.Interfaces;
        }

        public List<InterfaceInformation> GetInterfaces(ALProject project)
        {
            List<InterfaceInformation> infoList = new List<InterfaceInformation>();          
            foreach (ALAppInterface item in project.AllSymbols.Interfaces)
            {
                infoList.Add(new InterfaceInformation(item));
            }
            return infoList;
        }

    }
}
