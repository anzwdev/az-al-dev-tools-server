using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SortTableFieldsSyntaxRewriter : ALSyntaxRewriter
    {

        #region Table fields comparer

        protected class TableFieldComparer<T> : IComparer<T> where T : FieldBaseSyntax
         {

            public TableFieldComparer()
            {
            }

            protected long GetFieldId(FieldSyntax node)
            {
                if ((node != null) && (node.No != null) && (!String.IsNullOrWhiteSpace(node.No.ValueText)))
                {
                    long value;
                    if (Int64.TryParse(node.No.ValueText, out value))
                        return value;
                }
                return 0;
            }

            public int Compare(T x, T y)
            {
                FieldSyntax fieldX = x as FieldSyntax;
                FieldSyntax fieldY = y as FieldSyntax;

                long xNo = this.GetFieldId(fieldX);
                long yNo = this.GetFieldId(fieldY);

                int value = xNo.CompareTo(yNo);
                if (value != 0)
                    return value;

                string xName = x.GetNameStringValue();
                string yName = y.GetNameStringValue();

                if (xName != null)
                    return xName.CompareTo(yName);
                if (yName != null)
                    return -yName.CompareTo(xName);
                return 0;
            }
        }

        #endregion

        public SortTableFieldsSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitFieldList(FieldListSyntax node)
        {
            if ((node.Fields != null) && (node.Fields.Count > 0))
            {
                TableFieldComparer<FieldSyntax> comparer = new TableFieldComparer<FieldSyntax>();
                SyntaxList<FieldSyntax> fields = SyntaxNodesGroupsTree<FieldSyntax>.SortSyntaxList(
                    node.Fields, comparer);
                return node.WithFields(fields);
            }
            return base.VisitFieldList(node);
        }

        public override SyntaxNode VisitFieldExtensionList(FieldExtensionListSyntax node)
        {
            if ((node.Fields != null) && (node.Fields.Count > 0))
            {
                TableFieldComparer<FieldBaseSyntax> comparer = new TableFieldComparer<FieldBaseSyntax>();
                SyntaxList<FieldBaseSyntax> fields = SyntaxNodesGroupsTree<FieldBaseSyntax>.SortSyntaxList(
                    node.Fields, comparer);
                return node.WithFields(fields);
            }
            return base.VisitFieldExtensionList(node);
        }

    }
}
