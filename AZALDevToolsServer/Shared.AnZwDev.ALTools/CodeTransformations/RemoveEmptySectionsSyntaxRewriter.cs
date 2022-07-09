using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RemoveEmptySectionsSyntaxRewriter : ALSyntaxRewriter
    {

        public RemoveEmptySectionsSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitRequestPage(RequestPageSyntax node)
        {
            SyntaxNode newNode = base.VisitRequestPage(node);
            if (newNode is RequestPageSyntax processedNode)
            {
                if (
                    ((processedNode.PropertyList == null) || (processedNode.PropertyList.Properties == null) || (processedNode.PropertyList.Properties.Count == 0)) &&
                    ((processedNode.Layout == null) || (processedNode.Layout.Areas == null) || (processedNode.Layout.Areas.Count == 0)) &&
                    ((processedNode.Actions == null) || (processedNode.Actions.Areas == null) || (processedNode.Actions.Areas.Count == 0)) &&
                    ((processedNode.Members == null) || (processedNode.Members.Count == 0)) &&
                    (!processedNode.HasNonEmptyTrivia())
                )
                    return null;
            }
            return newNode;
        }

        public override SyntaxNode VisitPageLayout(PageLayoutSyntax node)
        {
            SyntaxNode newNode = base.VisitPageLayout(node);
            if (newNode is PageLayoutSyntax processedNode)
                if (((processedNode.Areas == null) || (processedNode.Areas.Count == 0)) && (!processedNode.HasNonEmptyTrivia()))
                    return null;
            return newNode;
        }

        public override SyntaxNode VisitPageActionList(PageActionListSyntax node)
        {
            SyntaxNode newNode = base.VisitPageActionList(node);
            if (newNode is PageActionListSyntax processedNode)
                if (((processedNode.Areas == null) || (processedNode.Areas.Count == 0)) && (!processedNode.HasNonEmptyTrivia()))
                    return null;
            return newNode;
        }

        public override SyntaxNode VisitPageActionArea(PageActionAreaSyntax node)
        {
            SyntaxNode newNode = base.VisitPageActionArea(node);
            if (newNode is PageActionAreaSyntax processedNode)
                if (((processedNode.Actions == null) || (processedNode.Actions.Count == 0)) && (!processedNode.HasNonEmptyTrivia()))
                return null;
            return newNode;
        }

        public override SyntaxNode VisitKeyList(KeyListSyntax node)
        {
            SyntaxNode newNode = base.VisitKeyList(node);
            if (newNode is KeyListSyntax processedNode)
                if (((processedNode.Keys == null) || (processedNode.Keys.Count == 0)) && (!processedNode.HasNonEmptyTrivia()))
                return null;
            return newNode;
        }

        public override SyntaxNode VisitFieldGroupList(FieldGroupListSyntax node)
        {
            SyntaxNode newNode = base.VisitFieldGroupList(node);
            if (newNode is FieldGroupListSyntax processedNode)
                if (((processedNode.FieldGroups == null) || (processedNode.FieldGroups.Count == 0)) && (!processedNode.HasNonEmptyTrivia()))
                return null;
            return newNode;
        }

    }
}
