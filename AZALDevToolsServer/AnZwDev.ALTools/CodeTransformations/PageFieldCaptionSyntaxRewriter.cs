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
    public class PageFieldCaptionSyntaxRewriter : BasePageWithSourceSyntaxRewriter
    {

        public bool UseNameIfNoCaption { get; set; }

        public PageFieldCaptionSyntaxRewriter()
        {
            this.UseNameIfNoCaption = false;
        }

        public override SyntaxNode VisitPageField(PageFieldSyntax node)
        {
            PropertySyntax propertySyntax = node.GetProperty("Caption");
            if ((propertySyntax == null) || (String.IsNullOrWhiteSpace(propertySyntax.Value.ToString())))
            {
                //try to find source field caption
                bool hasCaptionProperty;
                string caption = this.GetFieldCaption(node, out hasCaptionProperty);
                if ((!String.IsNullOrWhiteSpace(caption)) && 
                    ((hasCaptionProperty) || (this.UseNameIfNoCaption)))
                {
                    PropertySyntax newPropertySyntax = this.CreateCaptionProperty(node, caption);
                    NoOfChanges++;
                    if (propertySyntax == null)
                        return node.AddPropertyListProperties(newPropertySyntax);
                    return node.ReplaceNode(propertySyntax, newPropertySyntax);
                }
            }
            return base.VisitPageField(node);
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
