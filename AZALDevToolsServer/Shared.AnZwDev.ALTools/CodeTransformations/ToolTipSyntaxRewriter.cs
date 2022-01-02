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
    public class ToolTipSyntaxRewriter : BasePageWithSourceSyntaxRewriter
    {
        public string PageFieldTooltip { get; set; }
        public string PageFieldTooltipComment { get; set; }
        public string PageActionTooltip { get; set; }
        public bool UseFieldDescription { get; set; }
        
        public Dictionary<string, Dictionary<string, List<string>>> ToolTipsCache { get; set; }


        public ToolTipSyntaxRewriter()
        {
            this.PageActionTooltip = "Executes the %1 action.";
            this.PageFieldTooltip = "Specifies the value of %1 field.";
            this.PageFieldTooltipComment = "%Caption.Comment%";
            this.UseFieldDescription = false;
            this.ToolTipsCache = null;
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
            string forceToolTipValue = null;
            TableFieldCaptionInfo captionInfo = this.GetFieldCaption(node);
            if (this.UseFieldDescription)
                forceToolTipValue = captionInfo.Description;
            else if ((this.ToolTipsCache != null) && (!String.IsNullOrWhiteSpace(this.TableName)) && (!String.IsNullOrWhiteSpace(captionInfo.FieldName)))
            {
                //find first tooltip from other pages
                string tableNameKey = this.TableName.ToLower();
                if (this.ToolTipsCache.ContainsKey(tableNameKey))
                {
                    Dictionary<string, List<string>> tableToolTipsCache = this.ToolTipsCache[tableNameKey];
                    string fieldNameKey = captionInfo.FieldName.ToLower();
                    if (tableToolTipsCache.ContainsKey(fieldNameKey))
                    {
                        List<string> fieldToolTipsCache = tableToolTipsCache[fieldNameKey];
                        if (fieldToolTipsCache.Count > 0)
                            forceToolTipValue = fieldToolTipsCache[0];
                    }
                }
            }

            return node.AddPropertyListProperties(this.CreateToolTipProperty(node, captionInfo.Caption.Value, captionInfo.Caption.Comment, forceToolTipValue));
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
            return ((node.HasNonEmptyProperty("ToolTip")) || (node.HasProperty("ToolTipML")));
        }

        protected PropertySyntax CreateToolTipProperty(SyntaxNode node, string caption = null, string comment = null, string forceToolTipValue = null)
        {
            SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);

            string toolTipValue = "";
            string toolTipComment = "";

            if (String.IsNullOrWhiteSpace(forceToolTipValue))
            {
                //get caption from control caption
                LabelInformation controlCaptionInformation = node.GetCaptionPropertyInformation();
                if ((controlCaptionInformation != null) && (!String.IsNullOrWhiteSpace(controlCaptionInformation.Value)))
                {
                    caption = controlCaptionInformation.Value;
                    comment = controlCaptionInformation.Comment;
                }
                //get caption from control name
                else if (String.IsNullOrWhiteSpace(caption))
                {
                    caption = node.GetNameStringValue().RemovePrefixSuffix(this.Project.MandatoryPrefixes, this.Project.MandatorySuffixes, this.Project.MandatoryAffixes);
                    comment = null;
                }

                switch (node.Kind.ConvertToLocalType())
                {
                    case ConvertedSyntaxKind.PageField:
                        toolTipValue = PageFieldTooltip;
                        toolTipComment = PageFieldTooltipComment;
                        break;
                    case ConvertedSyntaxKind.PageAction:
                        toolTipValue = PageActionTooltip;
                        break;
                }

                toolTipValue = ApplyTextTemplate(toolTipValue, caption, comment);
                toolTipComment = ApplyTextTemplate(toolTipComment, caption, comment);
            }
            else
            {
                toolTipValue = forceToolTipValue;
            }

            return SyntaxFactoryHelper.ToolTipProperty(toolTipValue, toolTipComment, false)
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }

        protected string ApplyTextTemplate(string template, string caption, string comment)
        {
            if (template == null)
                return "";

            if (comment == null)
                comment = "";
            if (caption == null)
                caption = "";

            if (template.Contains("%1"))
                template = template.Replace("%1", caption);
            if (template.Contains("%Caption%"))
                template = template.Replace("%Caption%", caption);
            if (template.Contains("%Caption.Comment%"))
                template = template.Replace("%Caption.Comment%", comment);

            return template;
        }


    }
}
