using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class AppAreaSyntaxRewriter : ALSyntaxRewriter
    {

        public string ApplicationAreaName { get; set; }

        public AppAreaSyntaxRewriter()
        {
            this.ApplicationAreaName = null;
        }

        protected override SyntaxNode AfterVisitNode(SyntaxNode node)
        {
            if (this.NoOfChanges == 0)
                return null;
            return base.AfterVisitNode(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            if ((node.HasProperty("UsageCategory")) && (!this.HasApplicationArea(node)))
            {
                NoOfChanges++;
                node = node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
            }

            return base.VisitPage(node);
        }

        public override SyntaxNode VisitPageField(PageFieldSyntax node)
        {
            if (this.HasApplicationArea(node))
                return base.VisitPageField(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
        }

        public override SyntaxNode VisitPageUserControl(PageUserControlSyntax node)
        {
            if (this.HasApplicationArea(node))
                return base.VisitPageUserControl(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
        }

        public override SyntaxNode VisitPagePart(PagePartSyntax node)
        {
            if (this.HasApplicationArea(node))
                return base.VisitPagePart(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
        }

        public override SyntaxNode VisitPageSystemPart(PageSystemPartSyntax node)
        {
            if (this.HasApplicationArea(node))
                return base.VisitPageSystemPart(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
        }

        public override SyntaxNode VisitPageChartPart(PageChartPartSyntax node)
        {
            if (this.HasApplicationArea(node))
                return base.VisitPageChartPart(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
        }

        public override SyntaxNode VisitPageAction(PageActionSyntax node)
        {
            if (this.HasApplicationArea(node))
                return base.VisitPageAction(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node));
        }

        protected bool HasApplicationArea(SyntaxNode node)
        {
            return node.HasProperty("ApplicationArea");
        }

        protected PropertySyntax CreateApplicationAreaProperty(SyntaxNode node)
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
            values = values.Add(SyntaxFactory.IdentifierName(this.ApplicationAreaName));

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
