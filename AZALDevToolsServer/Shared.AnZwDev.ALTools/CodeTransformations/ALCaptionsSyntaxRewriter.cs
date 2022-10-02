﻿using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class ALCaptionsSyntaxRewriter : ALSyntaxRewriter
    {

        public ALCaptionsSyntaxRewriter()
        {
        }

        protected T UpdateCaptionFromName<T>(T node, PropertySyntax oldCaptionPropertySyntax, bool locked) where T : SyntaxNode
        {
            string valueText = oldCaptionPropertySyntax.Value.ToString();
            if (String.IsNullOrWhiteSpace(valueText))
            {
                NoOfChanges++;
                return node.ReplaceNode(oldCaptionPropertySyntax, this.CreateCaptionPropertyFromName(node, locked));
            }
            return node;
        }

        protected PropertySyntax CreateCaptionPropertyFromName(SyntaxNode node, bool locked)
        {
            string value = node.GetNameStringValue().RemovePrefixSuffix(
                this.Project.MandatoryPrefixes, this.Project.MandatorySuffixes, this.Project.MandatoryAffixes, this.Project.AdditionalMandatoryAffixesPatterns);

            SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);

            return SyntaxFactoryHelper.CaptionProperty(value, null, locked)
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }

    }
}
