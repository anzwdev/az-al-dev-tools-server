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

#if BC
        protected class VariableDeclarationNameComparer : IComparer<VariableDeclarationNameSyntax>
        {
            protected static AlphanumComparatorFast _stringComparer = new AlphanumComparatorFast();

            public int Compare(VariableDeclarationNameSyntax x, VariableDeclarationNameSyntax y)
            {
                string xName = x.Name?.Unquoted();
                string yName = y.Name?.Unquoted();
                return _stringComparer.Compare(xName, yName);
            }
        }

        protected class VariableComparer : IComparer<VariableDeclarationBaseSyntax>
        {
            protected static string[] _typePriority = {"record ", "report", "codeunit", "xmlport", "page", "query", "notification",
                    "bigtext", "dateformula", "recordid", "recordref", "fieldref", "filterpagebuilder" };
            protected static AlphanumComparatorFast _stringComparer = new AlphanumComparatorFast();

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
                        return node.Type.DataType.ToString().Replace("\"", "").ToLower().Trim();
                     return node.Type.ToString().ToLower().Replace("\"", "").Trim();
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

                int value = _stringComparer.Compare(xTypeName, yTypeName);
                if (value != 0)
                    return value;

                string xName = this.GetVariableName(x);
                string yName = this.GetVariableName(y);
                return _stringComparer.Compare(xName, yName);
            }

            protected string GetVariableName(VariableDeclarationBaseSyntax variableDeclarationBaseSyntax)
            {
                if (variableDeclarationBaseSyntax is VariableListDeclarationSyntax variableListDeclaration)
                {
                    if ((variableListDeclaration.VariableNames != null) && (variableListDeclaration.VariableNames.Count > 0))
                        return variableListDeclaration.VariableNames[0]?.Name?.Unquoted();
                }
                return variableDeclarationBaseSyntax.GetNameStringValue()?.ToLower();
            }


        }
#else
        protected class VariableComparer : IComparer<VariableDeclarationSyntax>
        {
            protected static string[] _typePriority = {"record ", "report", "codeunit", "xmlport", "page", "query", "notification",
                    "bigtext", "dateformula", "recordid", "recordref", "fieldref", "filterpagebuilder" };
            protected static AlphanumComparatorFast _stringComparer = new AlphanumComparatorFast();

            public VariableComparer()
            {
            }

            protected int GetDataTypePriority(string dataTypeName)
            {
                for (int i = 0; i < _typePriority.Length; i++)
                {
                    if (dataTypeName.StartsWith(_typePriority[i]))
                        return i;
                }
                return _typePriority.Length;
            }

            protected string GetDataTypeName(VariableDeclarationSyntax node)
            {
                if (node.Type != null)
                {
                    if (node.Type.DataType != null)
                        return node.Type.DataType.ToString().Replace("\"", "").ToLower().Trim();
                    return node.Type.ToString().ToLower().Replace("\"", "").Trim();
                }
                return "";
            }

            public int Compare(VariableDeclarationSyntax x, VariableDeclarationSyntax y)
            {
                string xTypeName = this.GetDataTypeName(x);
                string yTypeName = this.GetDataTypeName(y);

                //check type
                int xTypePriority = this.GetDataTypePriority(xTypeName);
                int yTypePriority = this.GetDataTypePriority(yTypeName);
                if (xTypePriority != yTypePriority)
                    return xTypePriority - yTypePriority;

                int value = _stringComparer.Compare(xTypeName, yTypeName);
                if (value != 0)
                    return value;

                string xName = x.GetNameStringValue().ToLower();
                string yName = y.GetNameStringValue().ToLower();
                return _stringComparer.Compare(xName, yName);
            }
        }
#endif

        #endregion

        public SortVariablesSyntaxRewriter()
        {
        }

#if BC
        public override SyntaxNode VisitGlobalVarSection(GlobalVarSectionSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
                node = node.WithVariables(this.SortVariables(node.Variables));
            return base.VisitGlobalVarSection(node);
        }
#endif

        public override SyntaxNode VisitVarSection(VarSectionSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
                node = node.WithVariables(this.SortVariables(node.Variables));
            return base.VisitVarSection(node);
        }

#if BC
        protected SyntaxList<VariableDeclarationBaseSyntax> SortVariables(SyntaxList<VariableDeclarationBaseSyntax> variables)
        {
            //sort variable names in variable list declarations
            bool anyNamesSorted = false;
            if ((variables != null) && (variables.Count > 0))
            {
                VariableDeclarationNameComparer variableNameComparer = new VariableDeclarationNameComparer();
                for (int i=0; i<variables.Count; i++)
                {
                    if (variables[i] is VariableListDeclarationSyntax variableListDeclaration)
                    {
                        (VariableListDeclarationSyntax newVariableListDeclaration, bool namesSorted) = this.SortVariableNames(variableListDeclaration, variableNameComparer);
                        if (namesSorted)
                        {
                            variables = variables.Replace(variableListDeclaration, newVariableListDeclaration);
                            anyNamesSorted = true;
                        }
                    }
                }
            }

            //sort variables
            var newVariables = SyntaxNodesGroupsTree<VariableDeclarationBaseSyntax>.SortSyntaxList(
                variables, new VariableComparer(), out bool sorted);
            if (sorted || anyNamesSorted)
                this.NoOfChanges++;
            return newVariables;
        }

        protected (VariableListDeclarationSyntax, bool) SortVariableNames(VariableListDeclarationSyntax variables, VariableDeclarationNameComparer comparer)
        {
            if ((variables.VariableNames != null) && (variables.VariableNames.Count > 1))
            {
                var newNames = SyntaxNodesGroupsTree<VariableDeclarationNameSyntax>.SortSeparatedSyntaxList(variables.VariableNames, comparer, out bool sorted);
                if (sorted)
                    return (variables.WithVariableNames(newNames), true);
            }
            return (variables, false);
        }


#else
        protected SyntaxList<VariableDeclarationSyntax> SortVariables(SyntaxList<VariableDeclarationSyntax> variables)
        {
            var newVariables = SyntaxNodesGroupsTree<VariableDeclarationSyntax>.SortSyntaxList(
                variables, new VariableComparer(), out bool sorted);
            if (sorted)
                this.NoOfChanges++;
            return newVariables;
        }
#endif

    }
}
