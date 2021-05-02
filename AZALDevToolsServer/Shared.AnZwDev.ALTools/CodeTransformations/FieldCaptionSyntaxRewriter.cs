using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class FieldCaptionSyntaxRewriter : ALCaptionsSyntaxRewriter
    {

        public FieldCaptionSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitField(FieldSyntax node)
        {
            PropertySyntax propertySyntax = node.GetProperty("Caption");
            if (propertySyntax == null)
            {
                NoOfChanges++;
                return node.AddPropertyListProperties(
                    this.CreateCaptionPropertyFromName(node));
            }
            else
            {
                string valueText = propertySyntax.Value.ToString();
                if (String.IsNullOrWhiteSpace(valueText))
                {
                    NoOfChanges++;
                    return node.ReplaceNode(propertySyntax, this.CreateCaptionPropertyFromName(node));
                }
            }
            return base.VisitField(node);
        }

    }
}
