using AnZwDev.ALTools.ALSymbolReferences;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class PermissionSetInformation : BaseObjectInformation
    {

        public PermissionSetInformation()
        {
        }

        public PermissionSetInformation(ALAppPermissionSet permissionSet) : base(permissionSet)
        {
        }

    }
}
