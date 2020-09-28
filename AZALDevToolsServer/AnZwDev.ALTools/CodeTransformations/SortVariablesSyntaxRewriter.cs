using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SortVariablesSyntaxRewriter: ALSyntaxRewriter
    {

        #region Variable comparer

        protected class VariableComparer : IComparer<VariableDeclarationBaseSyntax>
        {
            protected static string[] _typePriority = {"record ", "report", "codeunit", "xmlport", "page", "query", "notification",
                    "bigtext", "dateformula", "recordid", "recordref", "fieldref", "filterpagebuilder" };

            public VariableComparer()
            {
            }

            protected int GetDataTypePriority(string dataTypeName)
            {
                for (int i=0; i<_typePriority.Length; i++)
                {
                    if (dataTypeName.StartsWith(_typePriority[i]))
                        return i;
                }
                return _typePriority.Length;
            }

            protected string GetDataTypeName(VariableDeclarationBaseSyntax node)
            {
                if (node.Type != null)
                {
                    if (node.Type.DataType != null)
                        return node.Type.DataType.ToString().ToLower().Trim();
                     return node.Type.ToString().ToLower().Trim();
                }
                return "";
            }

            public int Compare(VariableDeclarationBaseSyntax x, VariableDeclarationBaseSyntax y)
            {
                string xTypeName = this.GetDataTypeName(x);
                string yTypeName = this.GetDataTypeName(y);
                
                //check type
                int xTypePriority = this.GetDataTypePriority(xTypeName);
                int yTypePriority = this.GetDataTypePriority(yTypeName);
                if (xTypePriority != yTypePriority)
                    return xTypePriority - yTypePriority;

                int value = xTypeName.CompareTo(yTypeName);
                if (value != 0)
                    return value;

                string xName = x.GetNameStringValue().ToLower();
                string yName = y.GetNameStringValue().ToLower();
                return xName.CompareTo(yName);
            }
        }

        #endregion

        public SortVariablesSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitGlobalVarSection(GlobalVarSectionSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
                node = node.WithVariables(this.SortVariables(node.Variables));
            return base.VisitGlobalVarSection(node);
        }

        public override SyntaxNode VisitVarSection(VarSectionSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
                node = node.WithVariables(this.SortVariables(node.Variables));
            return base.VisitVarSection(node);
        }

        protected SyntaxList<VariableDeclarationBaseSyntax> SortVariables(SyntaxList<VariableDeclarationBaseSyntax> variables)
        {
            List<VariableDeclarationBaseSyntax> list = variables.ToList();
            list.Sort(new VariableComparer());
            return SyntaxFactory.List(list);
        }

    }
}
