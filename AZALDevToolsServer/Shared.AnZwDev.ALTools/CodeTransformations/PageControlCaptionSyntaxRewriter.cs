using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
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
            if ((this.SetFieldsCaptions) && (!node.HasProperty("CaptionML")))
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                if ((propertySyntax == null) || (String.IsNullOrWhiteSpace(propertySyntax.Value.ToString())))
                {
                    //try to find source field caption
                    TableFieldCaptionInfo captionInfo = this.GetFieldCaption(node);
                    LabelInformation captionLabel = captionInfo.Caption;
                    if (!String.IsNullOrWhiteSpace(captionLabel.Value))
                    {
                        PropertySyntax newPropertySyntax = this.CreateCaptionProperty(node, captionLabel.Value, captionLabel.Comment);
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
            if ((this.SetGroupsCaptions) && (!node.HasProperty("CaptionML")))
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                if ((propertySyntax == null) || (String.IsNullOrWhiteSpace(propertySyntax.Value.ToString())))
                {
                    string caption = node.GetNameStringValue().RemovePrefixSuffix(this.Project.MandatoryPrefixes, this.Project.MandatorySuffixes, this.Project.MandatoryAffixes);
                    if (!String.IsNullOrWhiteSpace(caption))
                    {
                        PropertySyntax newPropertySyntax = this.CreateCaptionProperty(node, caption, null);
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
            if ((this.SetActionGroupsCaptions) && (!node.HasProperty("CaptionML")))
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                if ((propertySyntax == null) || (String.IsNullOrWhiteSpace(propertySyntax.Value.ToString())))
                {
                    string caption = node.GetNameStringValue().RemovePrefixSuffix(this.Project.MandatoryPrefixes, this.Project.MandatorySuffixes, this.Project.MandatoryAffixes);
                    if (!String.IsNullOrWhiteSpace(caption))
                    {
                        PropertySyntax newPropertySyntax = this.CreateCaptionProperty(node, caption, null);
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
            if ((this.SetActionsCaptions) && (!node.HasProperty("CaptionML")))
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                if ((propertySyntax == null) || (String.IsNullOrWhiteSpace(propertySyntax.Value.ToString())))
                {
                    string caption = node.GetNameStringValue().RemovePrefixSuffix(this.Project.MandatoryPrefixes, this.Project.MandatorySuffixes, this.Project.MandatoryAffixes);
                    if (!String.IsNullOrWhiteSpace(caption))
                    {
                        PropertySyntax newPropertySyntax = this.CreateCaptionProperty(node, caption, null);
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
            if ((this.SetPartsCaptions) && (!node.HasProperty("CaptionML")))
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                if ((propertySyntax == null) || (String.IsNullOrWhiteSpace(propertySyntax.Value.ToString())))
                {
                    string caption = ALSyntaxHelper.DecodeName(node.PartName.ToString());
                    if (String.IsNullOrWhiteSpace(caption))
                        caption = node.GetNameStringValue();
                    caption = caption.RemovePrefixSuffix(this.Project.MandatoryPrefixes, this.Project.MandatorySuffixes, this.Project.MandatoryAffixes);
                    if (!String.IsNullOrWhiteSpace(caption))
                    {
                        PropertySyntax newPropertySyntax = this.CreateCaptionProperty(node, caption, null);
                        NoOfChanges++;
                        if (propertySyntax == null)
                            return node.AddPropertyListProperties(newPropertySyntax);
                        return node.ReplaceNode(propertySyntax, newPropertySyntax);
                    }
                }
            }
            return base.VisitPagePart(node);
        }

        protected PropertySyntax CreateCaptionProperty(SyntaxNode node, string caption, string comment)
        {
            SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);

            return SyntaxFactoryHelper.CaptionProperty(caption, comment, false)
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }


    }
}
