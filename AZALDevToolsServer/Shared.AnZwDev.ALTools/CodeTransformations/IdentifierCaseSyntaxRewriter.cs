using AnZwDev.ALTools.Logging;
using AnZwDev.ALTools.ALSymbols.Internal;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.ALLanguageInformation;
using AnZwDev.ALTools.Workspace.SymbolsInformation;

namespace AnZwDev.ALTools.CodeTransformations
{
    /// <summary>
    /// Advanced case syntax rewriter
    /// Can fix case of variables, fields and function names
    /// It is much slower and complex solution than KeywordCaseSyntaxRewriter,
    /// but can fix much more cases
    /// </summary>
    public class IdentifierCaseSyntaxRewriter : KeywordCaseSyntaxRewriter
    {

        public SemanticModel SemanticModel { get; set; }

        private DotNetInformationProvider _dotNetInformationProvider;
        protected DotNetInformationProvider DotNetInformationProvider
        {
            get
            {
                if (_dotNetInformationProvider == null)
                    _dotNetInformationProvider = new DotNetInformationProvider();
                return _dotNetInformationProvider;
            }
        }

        public IdentifierCaseSyntaxRewriter()
        {
            _dotNetInformationProvider = null;
        }

        public override SyntaxNode VisitPageSystemPart(PageSystemPartSyntax node)
        {
            if ((node.SystemPartType != null) && (node.SystemPartType.Identifier != null) && (node.SystemPartType.Identifier.ValueText != null))
            {
                string partName = node.SystemPartType.Identifier.ValueText;
                string newName = SystemPartCaseInformation.Values.FixCase(partName);
                if (newName != partName)
                {
                    SyntaxToken identifier = node.SystemPartType.Identifier;
                    IdentifierNameSyntax systemPartType = node.SystemPartType.WithIdentifier(
                        SyntaxFactory.Identifier(newName)
                            .WithLeadingTrivia(identifier.LeadingTrivia)
                            .WithTrailingTrivia(identifier.TrailingTrivia));
                    node = node.WithSystemPartType(systemPartType);
                }
            }
            return base.VisitPageSystemPart(node);
        }

        public override SyntaxNode VisitDotNetTypeReference(DotNetTypeReferenceSyntax node)
        {
            if ((this.Project != null) && (node.DataType != null) && (node.DataType.Kind.ConvertToLocalType() == ConvertedSyntaxKind.SubtypedDataType))
            {
                SubtypedDataTypeSyntax dataType = node.DataType as SubtypedDataTypeSyntax;
                if ((dataType != null) && (dataType.Subtype != null) && (dataType.Subtype.Kind.ConvertToLocalType() == ConvertedSyntaxKind.ObjectReference))
                {
                    if ((dataType.Subtype.Identifier != null) && (dataType.Subtype.Identifier.Kind.ConvertToLocalType() == ConvertedSyntaxKind.IdentifierName))
                    {
                        IdentifierNameSyntax identifier = dataType.Subtype.Identifier as IdentifierNameSyntax;
                        if ((identifier != null) && (identifier.Identifier != null) && (identifier.Identifier.ValueText != null))
                        {
                            string typeAliasName = identifier.Identifier.ValueText;
                            DotNetTypeInformation typeInformation = this.DotNetInformationProvider.GetDotNetTypeInformation(this.Project, typeAliasName);
                            if ((typeInformation != null) && (typeInformation.AliasName != null) && (typeInformation.AliasName != typeAliasName))
                            {
                                
                                IdentifierNameSyntax newIdentifier = SyntaxFactory.IdentifierName(typeInformation.AliasName).WithTriviaFrom(identifier);
                                ObjectNameOrIdSyntax newSubType = dataType.Subtype.WithIdentifier(newIdentifier);
                                dataType = dataType.WithSubtype(newSubType);
                                node = node.WithDataType(dataType);
                            }
                        }
                    }
                }
            }

            return base.VisitDotNetTypeReference(node);
        }

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                try
                {
                    string prevName = node.Identifier.ValueText;
                    string newName = prevName;
                    if (!String.IsNullOrWhiteSpace(prevName))
                    {
                        bool updated = false;
                        
                        //Try special cases
                        if (node.Parent != null)
                        {
                            ConvertedSyntaxKind parentKind = node.Parent.Kind.ConvertToLocalType();
                            switch (parentKind)
                            {
                                case ConvertedSyntaxKind.DotNetTypeReference:
                                    //skip DotNetTypeReference becasue semantic model does not recognize them
                                    updated = true;
                                    break;
                                case ConvertedSyntaxKind.PageSystemPart:
                                    //skip page system parts because library incorrectly reports SystemPart second parameter as Control
                                    //It reports systempart(ControlName, ControlName) instead of systempart(ControlName, SystemPartName)
                                    updated = true;
                                    break;
                                case ConvertedSyntaxKind.PageArea:
                                    updated = PageAreaCaseInformation.Values.TryFixCase(ref newName);
                                    break;
                                case ConvertedSyntaxKind.PageActionArea:
                                    updated = PageActionAreaCaseInformation.Values.TryFixCase(ref newName);
                                    break;
                                case ConvertedSyntaxKind.CommaSeparatedPropertyValue:
                                    if ((node.Parent.Parent != null) &&
                                        (node.Parent.Parent.Kind.ConvertToLocalType() == ConvertedSyntaxKind.Property))
                                    {
                                        PropertySyntax property = node.Parent.Parent as PropertySyntax;
                                        if ((property != null) && (property.Name != null) && (property.Name.Identifier != null))
                                        {
                                            string propertyName = property.Name.Identifier.ValueText;
                                            if ((propertyName != null) && (propertyName.Equals("ApplicationArea", StringComparison.CurrentCultureIgnoreCase)))
                                            {
                                                newName = ApplicationAreaCaseInformation.Values.FixCase(newName);
                                                updated = true;
                                            }
                                        }
                                    }
                                    break;
                            }
                        }

                        //get symbol information
                        if (!updated)
                        {
                            SymbolInfo info = this.SemanticModel.GetSymbolInfo(node);
                            if ((info != null) && (info.Symbol != null))
                            {
                                ConvertedSymbolKind symbolKind = info.Symbol.Kind.ConvertToLocalType();
                                if ((symbolKind != ConvertedSymbolKind.NamedType) &&
                                    (symbolKind != ConvertedSymbolKind.DotNetAssembly) &&
                                    (symbolKind != ConvertedSymbolKind.DotNetPackage) &&
                                    (symbolKind != ConvertedSymbolKind.DotNetTypeDeclaration) &&
                                    (symbolKind != ConvertedSymbolKind.DotNet)) 
                                    newName = info.Symbol.Name;
                            }
                        }

                        if ((prevName != newName) && (!String.IsNullOrWhiteSpace(newName)))
                        {
                            SyntaxToken identifier = node.Identifier;
                            node = node.WithIdentifier(
                                SyntaxFactory.Identifier(newName)
                                    .WithLeadingTrivia(identifier.LeadingTrivia)
                                    .WithTrailingTrivia(identifier.TrailingTrivia));
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageLog.LogError(e);
                }
            }

            return base.VisitIdentifierName(node);
        }

        protected bool IsApplicationAreaValue(IdentifierNameSyntax node)
        {
            if ((node.Parent != null) && 
                (node.Parent.Kind.ConvertToLocalType() == ConvertedSyntaxKind.CommaSeparatedPropertyValue) && 
                (node.Parent.Parent != null) && 
                (node.Parent.Parent.Kind.ConvertToLocalType() == ConvertedSyntaxKind.Property))
            {
                PropertySyntax property = node.Parent.Parent as PropertySyntax;
                if ((property != null) && (property.Name != null) && (property.Name.Identifier != null))
                {
                    string propertyName = property.Name.Identifier.ValueText;
                    return ((propertyName != null) && (propertyName.Equals("ApplicationArea", StringComparison.CurrentCultureIgnoreCase)));
                }
            }
            return false;
        }


    }
}
