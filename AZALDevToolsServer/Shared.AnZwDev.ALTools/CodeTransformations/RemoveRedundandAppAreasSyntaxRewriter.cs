using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.Extensions;
using System.Linq;
using System.Xml.Linq;
using AnZwDev.ALTools.Workspace;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RemoveRedundandAppAreasSyntaxRewriter : ALSyntaxRewriter
    {

        private enum PageMembersAppAreaState
        {
            NotChecked = 0,
            Equal = 1,
            Different = 2
        }

        private string _pageAppArea = null;
        private string _pageMembersAppArea = null;
        private PageMembersAppAreaState _pageMembersAppAreaState = PageMembersAppAreaState.NotChecked;

        public RemoveRedundandAppAreasSyntaxRewriter()
        {
        }

        protected override SyntaxNode AfterVisitNode(SyntaxNode node)
        {
            if (this.NoOfChanges == 0)
                return null;
            return base.AfterVisitNode(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            _pageAppArea = GetApplicationAreaValue(node);
            _pageMembersAppArea = null;
            _pageMembersAppAreaState = PageMembersAppAreaState.NotChecked;

            var newNode = base.VisitPage(node);

            node = newNode as PageSyntax;
            if ((node != null) && (String.IsNullOrWhiteSpace(_pageAppArea)) && (!String.IsNullOrWhiteSpace(_pageMembersAppArea)) && (_pageMembersAppAreaState == PageMembersAppAreaState.Equal))
            {
                NoOfChanges++;
                node = node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node, _pageMembersAppArea));
                _pageAppArea = _pageMembersAppArea;

                //run processing again to remove redundant application areas
                return base.VisitPage(node);
            }

            return newNode;
        }

        public override SyntaxNode VisitPageLabel(PageLabelSyntax node)
        {
            (var propertyList, var propertyListUpdated) = CheckAndRemovePageMemberAppArea(node, node.PropertyList);
            if (propertyListUpdated)
                node = node.WithPropertyList(propertyList);
            return base.VisitPageLabel(node);
        }

        public override SyntaxNode VisitPageField(PageFieldSyntax node)
        {
            (var propertyList, var propertyListUpdated) = CheckAndRemovePageMemberAppArea(node, node.PropertyList);
            if (propertyListUpdated)
                node = node.WithPropertyList(propertyList);
            return base.VisitPageField(node);
        }

        public override SyntaxNode VisitPageUserControl(PageUserControlSyntax node)
        {
            (var propertyList, var propertyListUpdated) = CheckAndRemovePageMemberAppArea(node, node.PropertyList);
            if (propertyListUpdated)
                node = node.WithPropertyList(propertyList);
            return base.VisitPageUserControl(node);
        }

        public override SyntaxNode VisitPagePart(PagePartSyntax node)
        {
            (var propertyList, var propertyListUpdated) = CheckAndRemovePageMemberAppArea(node, node.PropertyList);
            if (propertyListUpdated)
                node = node.WithPropertyList(propertyList);
            return base.VisitPagePart(node);
        }

        public override SyntaxNode VisitPageSystemPart(PageSystemPartSyntax node)
        {
            (var propertyList, var propertyListUpdated) = CheckAndRemovePageMemberAppArea(node, node.PropertyList);
            if (propertyListUpdated)
                node = node.WithPropertyList(propertyList);
            return base.VisitPageSystemPart(node);
        }

#if BC
        public override SyntaxNode VisitPageChartPart(PageChartPartSyntax node)
        {
            (var propertyList, var propertyListUpdated) = CheckAndRemovePageMemberAppArea(node, node.PropertyList);
            if (propertyListUpdated)
                node = node.WithPropertyList(propertyList);
            return base.VisitPageChartPart(node);
        }
#endif

        public override SyntaxNode VisitPageAction(PageActionSyntax node)
        {
            (var propertyList, var propertyListUpdated) = CheckAndRemovePageMemberAppArea(node, node.PropertyList);
            if (propertyListUpdated)
                node = node.WithPropertyList(propertyList);
            return base.VisitPageAction(node);
        }

        private (PropertyListSyntax, bool) CheckAndRemovePageMemberAppArea(SyntaxNode node, PropertyListSyntax propertyList)
        {
            PropertySyntax appAreaProperty = node.GetProperty("ApplicationArea");
            string memberAppArea = ALSyntaxHelper.DecodeName(appAreaProperty?.Value?.ToString());

            CheckIfMemberAppAreasAreTheSame(memberAppArea);

            if (ShouldRemoveAppArea(memberAppArea))
                return (RemoveProperty(propertyList, appAreaProperty), true);

            return (propertyList, false);
        }

        private bool ShouldRemoveAppArea(string memberAppArea)
        {
            return
                (!String.IsNullOrWhiteSpace(_pageAppArea)) &&
                (!String.IsNullOrWhiteSpace(memberAppArea)) &&
                (memberAppArea.Equals(_pageAppArea, StringComparison.CurrentCultureIgnoreCase));
        }

        private PropertyListSyntax RemoveProperty(PropertyListSyntax propertyList, PropertySyntax property)
        {
            NoOfChanges++;
            return propertyList.WithProperties(
                    propertyList.Properties.Remove(property));
        }

        private void CheckIfMemberAppAreasAreTheSame(string memberAppArea)
        {
            switch (_pageMembersAppAreaState)
            {
                case PageMembersAppAreaState.NotChecked:
                    _pageMembersAppArea = memberAppArea;
                    _pageMembersAppAreaState = PageMembersAppAreaState.Equal;
                    break;
                case PageMembersAppAreaState.Equal:
                    var equal =
                        ((memberAppArea == null) && (_pageMembersAppArea == null)) ||
                        ((memberAppArea != null) && (_pageMembersAppArea != null) && (memberAppArea.Equals(_pageMembersAppArea, StringComparison.CurrentCultureIgnoreCase)));
                    if (!equal)
                    {
                        _pageMembersAppArea = null;
                        _pageMembersAppAreaState = PageMembersAppAreaState.Different;
                    }    
                    break;
            }
        }

        protected string GetApplicationAreaValue(SyntaxNode node)
        {
            PropertySyntax appAreaProperty = node.GetProperty("ApplicationArea");
            if (appAreaProperty != null)
                return ALSyntaxHelper.DecodeName(appAreaProperty.Value?.ToString());
            return null;
        }

        protected PropertySyntax CreateApplicationAreaProperty(SyntaxNode node, string value)
        {
            //calculate indent
            int indentLength = 4;
            string indent;
            SyntaxTriviaList leadingTrivia = node.GetLeadingTrivia();
            if (leadingTrivia != null)
            {
                indent = leadingTrivia.ToString();
                int newLinePos = indent.LastIndexOf("/n");
                if (newLinePos >= 0)
                    indent = indent.Substring(newLinePos + 1);
                indentLength += indent.Length;
            }
            indent = "".PadLeft(indentLength);

            SyntaxTriviaList leadingTriviaList = SyntaxFactory.ParseLeadingTrivia(indent, 0);
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);

            SeparatedSyntaxList<IdentifierNameSyntax> values = new SeparatedSyntaxList<IdentifierNameSyntax>();
            values = values.Add(SyntaxFactory.IdentifierName(value));

            //try to convert from string to avoid issues with enum ids changed between AL compiler versions
            PropertyKind propertyKind;
            try
            {
                propertyKind = (PropertyKind)Enum.Parse(typeof(PropertyKind), "ApplicationArea", true);
            }
            catch (Exception)
            {
                propertyKind = PropertyKind.ApplicationArea;
            }

            return SyntaxFactory.Property(propertyKind, SyntaxFactory.CommaSeparatedPropertyValue(values))
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }


    }
}
