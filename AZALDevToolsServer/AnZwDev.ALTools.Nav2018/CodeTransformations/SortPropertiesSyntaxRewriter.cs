/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.CodeTransformations
{
    public class SortPropertiesSyntaxRewriter: ALSyntaxRewriter
    {

        #region Properties comparer

        protected class PropertyComparer : IComparer<SyntaxNodeSortInfo<PropertySyntax>>
        {
            public PropertyComparer()
            {
            }

            public int Compare(SyntaxNodeSortInfo<PropertySyntax> x, SyntaxNodeSortInfo<PropertySyntax> y)
            {
                int val = x.Name.CompareTo(y.Name);
                if (val != 0)
                    return val;
                return x.Index - y.Index;
            }
        }

        #endregion

        public SortPropertiesSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitPropertyList(PropertyListSyntax node)
        {
            if ((node.Properties != null) && (node.Properties.Count > 0))
            {
                List<SyntaxNodeSortInfo<PropertySyntax>> list =
                    SyntaxNodeSortInfo<PropertySyntax>.FromSyntaxList(node.Properties);
                list.Sort(new PropertyComparer());
                SyntaxList<PropertySyntax> properties =
                    SyntaxNodeSortInfo<PropertySyntax>.ToSyntaxList(list);
                node = node.WithProperties(properties);
            }
            return base.VisitPropertyList(node);
        }

    }
}
