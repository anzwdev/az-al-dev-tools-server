using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SortPropertiesSyntaxRewriter: ALSyntaxRewriter
    {

        #region Properties comparer

        protected class PropertyComparer : IComparer<SyntaxNodeSortInfo<PropertySyntaxOrEmpty>>
        {
            public PropertyComparer()
            {
            }

            public int Compare(SyntaxNodeSortInfo<PropertySyntaxOrEmpty> x, SyntaxNodeSortInfo<PropertySyntaxOrEmpty> y)
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
                List<SyntaxNodeSortInfo<PropertySyntaxOrEmpty>> list =
                    SyntaxNodeSortInfo<PropertySyntaxOrEmpty>.FromSyntaxList(node.Properties);
                list.Sort(new PropertyComparer());
                SyntaxList<PropertySyntaxOrEmpty> properties =
                    SyntaxNodeSortInfo<PropertySyntaxOrEmpty>.ToSyntaxList(list);
                node = node.WithProperties(properties);
            }
            return base.VisitPropertyList(node);
        }

    }
}
