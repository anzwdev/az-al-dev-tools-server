using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.TypeInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class ToolTipSyntaxRewriter : ALSyntaxRewriter
    {
        public string PageFieldTooltip { get; set; }
        public string PageActionTooltip { get; set; }
        public TypeInformationCollector TypeInformationCollector { get; }
        public TableTypeInformation CurrentTable { get; set; }

        public ToolTipSyntaxRewriter()
        {
            PageActionTooltip = "Executes the %1 action.";
            PageFieldTooltip = "Specifies the value of %1 field.";
            this.TypeInformationCollector = new TypeInformationCollector();
        }

        public override SyntaxNode VisitRequestPage(RequestPageSyntax node)
        {
            this.CheckSourceTableProperty(node);
            return base.VisitRequestPage(node);
        }

        public override SyntaxNode VisitPageExtension(PageExtensionSyntax node)
        {
            this.CurrentTable = null;
            return base.VisitPageExtension(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            this.CheckSourceTableProperty(node);
            return base.VisitPage(node);
        }

        private void CheckSourceTableProperty(SyntaxNode node)
        {
            //try to find current table
            this.CurrentTable = null;
            PropertyValueSyntax sourceTablePropertyValue = node.GetPropertyValue("SourceTable");
            if (sourceTablePropertyValue != null)
            {
                string sourceTable = ALSyntaxHelper.DecodeName(sourceTablePropertyValue.ToString());
                this.CurrentTable = this.TypeInformationCollector.ProjectTypesInformation.GetTable(sourceTable);
            }
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
            string caption = null;
            if (node.Expression != null)
            {
                string source = ALSyntaxHelper.DecodeName(node.Expression.ToString());
                if (!String.IsNullOrWhiteSpace(source))
                {
                    if (this.CurrentTable != null)
                    {
                        TableFieldTypeInformation tableField = this.CurrentTable.GetField(source);
                        if ((tableField != null) && (!String.IsNullOrWhiteSpace(tableField.Caption)))
                            caption = tableField.Caption;
                    }
                    if (String.IsNullOrWhiteSpace(caption))
                        caption = source.Replace("\"", "");
                }
            }



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

            return SyntaxFactory.PropertyLiteral(PropertyKind.ToolTip, toolTipValue)
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }

    }
}
