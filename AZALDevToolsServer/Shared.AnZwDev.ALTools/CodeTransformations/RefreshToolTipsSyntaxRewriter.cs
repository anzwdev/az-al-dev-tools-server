using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RefreshToolTipsSyntaxRewriter : BasePageWithSourceSyntaxRewriter
    {

        public Dictionary<string, Dictionary<string, List<string>>> ToolTipsCache { get; set; }

        public RefreshToolTipsSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitPageField(PageFieldSyntax node)
        {
            if ((!node.ContainsDiagnostics) && (this.ToolTipsCache != null) && (!String.IsNullOrWhiteSpace(this.TableName)) && (!node.HasProperty("ToolTipML")))
            {
                //find first tooltip from other pages
                TableFieldCaptionInfo captionInfo = this.GetFieldCaption(node);
                string tableNameKey = this.TableName.ToLower();
                if (this.ToolTipsCache.ContainsKey(tableNameKey))
                {
                    Dictionary<string, List<string>> tableToolTipsCache = this.ToolTipsCache[tableNameKey];
                    string fieldNameKey = captionInfo.FieldName.ToLower();
                    if (tableToolTipsCache.ContainsKey(fieldNameKey))
                    {
                        List<string> fieldToolTipsCache = tableToolTipsCache[fieldNameKey];
                        if ((fieldToolTipsCache.Count > 0) && (!String.IsNullOrWhiteSpace(fieldToolTipsCache[0])))
                        {
                            string newToolTip = fieldToolTipsCache[0];
                            
                            PropertySyntax propertySyntax = node.GetProperty("ToolTip");
                            if (propertySyntax == null)
                            {
                                NoOfChanges++;
                                SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
                                SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);
                                return node.AddPropertyListProperties(
                                    SyntaxFactoryHelper.ToolTipProperty(newToolTip, "", false)
                                        .WithLeadingTrivia(leadingTriviaList)
                                        .WithTrailingTrivia(trailingTriviaList));
                            }
                            else
                            {
                                NoOfChanges++;
                                PropertySyntax newPropertySyntax = SyntaxFactoryHelper.ToolTipProperty(newToolTip, "", false)
                                    .WithTriviaFrom(propertySyntax);
                                return node.ReplaceNode(propertySyntax, newPropertySyntax);
                            }
                        }
                    }
                }
            }

            return base.VisitPageField(node);
        }

        protected PropertySyntax CreateToolTipProperty(string toolTipValue, SyntaxTriviaList leadingTriviaList, SyntaxTriviaList trailingTriviaList)
        {
            return SyntaxFactoryHelper.ToolTipProperty(toolTipValue, "", false)
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }

    }
}
