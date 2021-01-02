using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SortReportColumnsSyntaxRewriter: ALSyntaxRewriter
    {

        protected class ReportElementComparer : IComparer<SyntaxNodeSortInfo<ReportDataItemElementSyntax>>
        {
            protected static AlphanumComparatorFast _stringComparer = new AlphanumComparatorFast();

            public ReportElementComparer()
            {
            }

            public int Compare(SyntaxNodeSortInfo<ReportDataItemElementSyntax> x, SyntaxNodeSortInfo<ReportDataItemElementSyntax> y)
            {
                if ((x.Kind == y.Kind) && (x.Kind == ConvertedSyntaxKind.ReportColumn))
                    return _stringComparer.Compare(x.Name, y.Name);
                if (x.Kind == ConvertedSyntaxKind.ReportColumn)
                    return -1;
                if (y.Kind == ConvertedSyntaxKind.ReportColumn)
                    return 1;
                return (x.Index - y.Index);
            }
        }

        public SortReportColumnsSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitReportDataItem(ReportDataItemSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics) && (node.Elements != null) && (node.Elements.Count > 1))
            {
                SyntaxList<ReportDataItemElementSyntax> elements =
                    SyntaxNodesGroupsTree<ReportDataItemElementSyntax>.SortSyntaxListWithSortInfo(
                        node.Elements, new ReportElementComparer());
                node = node.WithElements(elements);
            }
            return base.VisitReportDataItem(node);
        }

    }
}
