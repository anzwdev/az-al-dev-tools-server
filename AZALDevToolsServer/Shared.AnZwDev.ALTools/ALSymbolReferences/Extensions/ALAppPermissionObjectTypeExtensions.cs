using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.Extensions
{
    public static class ALAppPermissionObjectTypeExtensions
    {

        public static ALAppPermissionObjectType FromString(string source)
        {
            if (source != null)
            {
                source = source.ToLower();
                switch (source)
                {
                    case "tabledata":
                        return ALAppPermissionObjectType.TableData;
                    case "table":
                        return ALAppPermissionObjectType.Table;
                    case "report":
                        return ALAppPermissionObjectType.Report;
                    case "codeunit":
                        return ALAppPermissionObjectType.Codeunit;
                    case "xmlport":
                        return ALAppPermissionObjectType.XmlPort;
                    case "page":
                        return ALAppPermissionObjectType.Page;
                    case "query":
                        return ALAppPermissionObjectType.Query;
                    case "system":
                        return ALAppPermissionObjectType.System;
                }
            }
            return ALAppPermissionObjectType.Undefined;
        }

    }
}
