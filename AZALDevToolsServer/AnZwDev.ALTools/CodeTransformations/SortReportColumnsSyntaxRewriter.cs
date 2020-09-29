﻿using AnZwDev.ALTools.ALSymbols.Internal;
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
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
            {
                List<SyntaxNodeSortInfo<ReportDataItemElementSyntax>> list =
                SyntaxNodeSortInfo<ReportDataItemElementSyntax>.FromSyntaxList(node.Elements);
                if (list.Count > 1)
                {
                    list.Sort(new ReportElementComparer());
                    List<ReportDataItemElementSyntax> elementsList = new List<ReportDataItemElementSyntax>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        elementsList.Add(list[i].Node);
                    }
                    node = node.WithElements(SyntaxFactory.List<ReportDataItemElementSyntax>(elementsList));
                }
            }
            return base.VisitReportDataItem(node);
        }

    }
}
