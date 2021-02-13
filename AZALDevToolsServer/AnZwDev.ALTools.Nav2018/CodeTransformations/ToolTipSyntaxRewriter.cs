/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018.ALSymbols;
using AnZwDev.ALTools.Nav2018.ALSymbols.Internal;
using AnZwDev.ALTools.Nav2018.Extensions;
using AnZwDev.ALTools.Nav2018.TypeInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.CodeTransformations
{
    public class ToolTipSyntaxRewriter : BasePageWithSourceSyntaxRewriter
    {
        public string PageFieldTooltip { get; set; }
        public string PageActionTooltip { get; set; }

        public ToolTipSyntaxRewriter()
        {
            PageActionTooltip = "Executes the %1 action.";
            PageFieldTooltip = "Specifies the value of %1 field.";
        }

        protected override SyntaxNode AfterVisitNode(SyntaxNode node)
        {
            if (this.NoOfChanges == 0)
                return null;
            return base.AfterVisitNode(node);
        }

        public override SyntaxNode VisitPageField(PageFieldSyntax node)
        {
            if (this.HasToolTip(node))
                return base.VisitPageField(node);
            this.NoOfChanges++;

            //try to find source field caption
            string caption = this.GetFieldCaption(node, out _);

            return node.AddPropertyListProperties(this.CreateToolTipProperty(node, caption));
        }

        public override SyntaxNode VisitPageAction(PageActionSyntax node)
        {
            if (this.HasToolTip(node))
                return base.VisitPageAction(node);
            this.NoOfChanges++;
            return node.AddPropertyListProperties(this.CreateToolTipProperty(node));
        }

        protected bool HasToolTip(SyntaxNode node)
        {
            PropertySyntax ToolTipProperty = node.GetProperty("ToolTip");
            return ((ToolTipProperty != null) && (!String.IsNullOrWhiteSpace(ToolTipProperty.Value.ToString())));
        }

        protected PropertySyntax CreateToolTipProperty(SyntaxNode node, string caption = null)
        {
            SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);

            //get caption from control
            PropertyValueSyntax captionProperty = node.GetPropertyValue("Caption");
            if (captionProperty != null)
            {
                string value = ALSyntaxHelper.DecodeString(captionProperty.ToString());
                if (!String.IsNullOrWhiteSpace(value))
                    caption = value;
            }
            else if (String.IsNullOrWhiteSpace(caption))
                caption = node.GetNameStringValue();


            string toolTipValue = "";
            switch (node.Kind.ConvertToLocalType())
            {
                case ConvertedSyntaxKind.PageField:
                    toolTipValue = PageFieldTooltip;
                    break;
                case ConvertedSyntaxKind.PageAction:
                    toolTipValue = PageActionTooltip;
                    break;
            }

            if (toolTipValue.Contains("%1"))
                toolTipValue = toolTipValue.Replace("%1", caption);

            //try to convert from string to avoid issues with enum ids changed between AL compiler versions
            PropertyKind propertyKind;
            try
            {
                propertyKind = (PropertyKind)Enum.Parse(typeof(PropertyKind), "ToolTip", true);
            }
            catch (Exception)
            {
                propertyKind = PropertyKind.ToolTip;
            }

            return SyntaxFactory.PropertyLiteral(propertyKind, toolTipValue)
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }

    }
}
