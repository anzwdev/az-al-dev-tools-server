using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class PageControlCaptionSyntaxRewriter : BasePageWithSourceSyntaxRewriter
    {

        public bool SetActionsCaptions { get; set; }
        public bool SetGroupsCaptions { get; set; }
        public bool SetActionGroupsCaptions { get; set; }
        public bool SetPartsCaptions { get; set; }
        public bool SetFieldsCaptions { get; set; }

        public PageControlCaptionSyntaxRewriter()
        {
            SetActionsCaptions = false;
            SetGroupsCaptions = false;
            SetPartsCaptions = false;
            SetFieldsCaptions = false;
        }

        public override SyntaxNode VisitPageField(PageFieldSyntax node)
        {
            if (this.SetFieldsCaptions)
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                if ((propertySyntax == null) || (String.IsNullOrWhiteSpace(propertySyntax.Value.ToString())))
                {
                    //try to find source field caption
                    bool hasCaptionProperty;
                    string caption = this.GetFieldCaption(node, out hasCaptionProperty);
                    if (!String.IsNullOrWhiteSpace(caption))
                    {
                        PropertySyntax newPropertySyntax = this.CreateCaptionProperty(node, caption);
                        NoOfChanges++;
                        if (propertySyntax == null)
                            return node.AddPropertyListProperties(newPropertySyntax);
                        return node.ReplaceNode(propertySyntax, newPropertySyntax);
                    }
                }
            }
            return base.VisitPageField(node);
        }

        public override SyntaxNode VisitPageGroup(PageGroupSyntax node)
        {
            if (this.SetGroupsCaptions)
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                if ((propertySyntax == null) || (String.IsNullOrWhiteSpace(propertySyntax.Value.ToString())))
                {
                    string caption = node.GetNameStringValue();
                    if (!String.IsNullOrWhiteSpace(caption))
                    {
                        PropertySyntax newPropertySyntax = this.CreateCaptionProperty(node, caption);
                        NoOfChanges++;
                        if (propertySyntax == null)
                            node = node.AddPropertyListProperties(newPropertySyntax);
                        else
                            node = node.ReplaceNode(propertySyntax, newPropertySyntax);
                    }
                }
            }
            return base.VisitPageGroup(node);
        }

        public override SyntaxNode VisitPageActionGroup(PageActionGroupSyntax node)
        {
            if (this.SetActionGroupsCaptions)
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                if ((propertySyntax == null) || (String.IsNullOrWhiteSpace(propertySyntax.Value.ToString())))
                {
                    string caption = node.GetNameStringValue();
                    if (!String.IsNullOrWhiteSpace(caption))
                    {
                        PropertySyntax newPropertySyntax = this.CreateCaptionProperty(node, caption);
                        NoOfChanges++;
                        if (propertySyntax == null)
                            node = node.AddPropertyListProperties(newPropertySyntax);
                        else
                            node = node.ReplaceNode(propertySyntax, newPropertySyntax);
                    }
                }
            }
            return base.VisitPageActionGroup(node);
        }

        public override SyntaxNode VisitPageAction(PageActionSyntax node)
        {
            if (this.SetActionsCaptions)
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                if ((propertySyntax == null) || (String.IsNullOrWhiteSpace(propertySyntax.Value.ToString())))
                {
                    string caption = node.GetNameStringValue();
                    if (!String.IsNullOrWhiteSpace(caption))
                    {
                        PropertySyntax newPropertySyntax = this.CreateCaptionProperty(node, caption);
                        NoOfChanges++;
                        if (propertySyntax == null)
                            return node.AddPropertyListProperties(newPropertySyntax);
                        return node.ReplaceNode(propertySyntax, newPropertySyntax);
                    }
                }
            }
            return base.VisitPageAction(node);
        }

        public override SyntaxNode VisitPagePart(PagePartSyntax node)
        {
            if (this.SetPartsCaptions)
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                if ((propertySyntax == null) || (String.IsNullOrWhiteSpace(propertySyntax.Value.ToString())))
                {
                    string caption = ALSyntaxHelper.DecodeName(node.PartName.ToString());
                    if (String.IsNullOrWhiteSpace(caption))
                        caption = node.GetNameStringValue();
                    if (!String.IsNullOrWhiteSpace(caption))
                    {
                        PropertySyntax newPropertySyntax = this.CreateCaptionProperty(node, caption);
                        NoOfChanges++;
                        if (propertySyntax == null)
                            return node.AddPropertyListProperties(newPropertySyntax);
                        return node.ReplaceNode(propertySyntax, newPropertySyntax);
                    }
                }
            }
            return base.VisitPagePart(node);
        }

        protected PropertySyntax CreateCaptionProperty(SyntaxNode node, string caption)
        {
            SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);

            PropertyKind propertyKind;
            try
            {
                propertyKind = (PropertyKind)Enum.Parse(typeof(PropertyKind), "Caption", true);
            }
            catch (Exception)
            {
                propertyKind = PropertyKind.Caption;
            }

            return SyntaxFactory.PropertyLiteral(propertyKind, caption)
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }


    }
}
