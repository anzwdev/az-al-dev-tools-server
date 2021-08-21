using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class PermissionComparer : IComparer<PermissionSyntax>
    {

        private Dictionary<string, int> _typePriorities;
        protected static AlphanumComparatorFast _stringComparer = new AlphanumComparatorFast();

        public PermissionComparer()
        {
            this._typePriorities = new Dictionary<string, int>();
            this._typePriorities.Add("table", 0);
            this._typePriorities.Add("tabledata", 1);
            this._typePriorities.Add("codeunit", 2);
            this._typePriorities.Add("page", 3);
            this._typePriorities.Add("report", 4);
            this._typePriorities.Add("xmlport", 5);
            this._typePriorities.Add("query", 6);
        }

        public int Compare(PermissionSyntax x, PermissionSyntax y)
        {
            int xType = this.GetTypePriority(ALSyntaxHelper.DecodeName(x.ObjectType.ToString()));
            string xName = ALSyntaxHelper.DecodeName(x.ObjectReference.Identifier.ToString());

            int yType = this.GetTypePriority(ALSyntaxHelper.DecodeName(y.ObjectType.ToString()));
            string yName = ALSyntaxHelper.DecodeName(y.ObjectReference.Identifier.ToString());

            if (xType != yType)
                return xType - yType;

            return _stringComparer.Compare(xName, yName);
        }

        protected int GetTypePriority(string type)
        {
            if (type != null)
            {
                type = type.ToLower();
                if (this._typePriorities.ContainsKey(type))
                    return this._typePriorities[type];
            }
            return 0;
        }
    }

}
