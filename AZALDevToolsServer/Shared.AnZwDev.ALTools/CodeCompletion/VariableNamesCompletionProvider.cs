﻿using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;

namespace AnZwDev.ALTools.CodeCompletion
{
    public class VariableNamesCompletionProvider : CodeCompletionProvider
    {

        private bool IncludeDataType { get; }

        private static string _variableNamesWithTypeName = "VariableNamesWithType";
        private static string _variableNamesName = "VariableNames";
        private static string _tempPrefix = "Temp";
        private static string[] _fullDeclarationCommitCharacters = { ";" };

        public VariableNamesCompletionProvider(ALDevToolsServer server, bool includeDataType) : base(server, includeDataType ? _variableNamesWithTypeName : _variableNamesName)
        {
            IncludeDataType = includeDataType;
        }

        public override bool CanBeUsed(HashSet<string> providerNames)
        {
            return
                providerNames.Contains(Name) &&
                ((Name != _variableNamesName) || (!providerNames.Contains(_variableNamesWithTypeName)));
        }

        public override void CollectCompletionItems(ALProject project, SyntaxTree syntaxTree, SyntaxNode syntaxNode, int position, List<CodeCompletionItem> completionItems)
        {
            if (ValidSyntaxNode(syntaxNode, position))
            {
                CreateCompletionItems(project, project.AllSymbols.Tables.GetObjects(), completionItems);
                CreateCompletionItems(project, project.AllSymbols.Tables.GetObjects(), completionItems, true);
                CreateCompletionItems(project, project.AllSymbols.Codeunits.GetObjects(), completionItems);
                CreateCompletionItems(project, project.AllSymbols.Pages.GetObjects(), completionItems);
                CreateCompletionItems(project, project.AllSymbols.Reports.GetObjects(), completionItems);
                CreateCompletionItems(project, project.AllSymbols.Queries.GetObjects(), completionItems);
                CreateCompletionItems(project, project.AllSymbols.XmlPorts.GetObjects(), completionItems);
                CreateCompletionItems(project, project.AllSymbols.EnumTypes.GetObjects(), completionItems);
                CreateCompletionItems(project, project.AllSymbols.Interfaces.GetObjects(), completionItems);
            }
        }

        private bool ValidSyntaxNode(SyntaxNode syntaxNode, int position)
        {
            switch (syntaxNode)
            {
#if BC
                case VarSectionBaseSyntax varSection:
#else
                case VarSectionSyntax varSection:
#endif
                    return (varSection.VarKeyword != null) && (varSection.VarKeyword.Span.End < position);
                case BlockSyntax blockSyntax:
                    return (blockSyntax.BeginKeywordToken != null) && (blockSyntax.BeginKeywordToken.Span.Start > position);
                case ParameterListSyntax parameterListSyntax:
                    var parameterSyntax = FindParameterSyntax(parameterListSyntax, position);
                    return
                        (
                            (parameterSyntax == null) &&
                            (parameterListSyntax.OpenParenthesisToken.IsEmptyOrBefore(position)) &&
                            (parameterListSyntax.CloseParenthesisToken.IsEmptyOrAfter(position))
                        ) || (
                            (parameterSyntax != null) &&
                            (ValidDeclarationNode(parameterSyntax, parameterSyntax.Name, position))
                        );
                default:
                    var declarationSyntaxNode = syntaxNode.FindParentByKind(ConvertedSyntaxKind.VariableDeclaration, ConvertedSyntaxKind.Parameter, ConvertedSyntaxKind.ReturnValue);
                    var nameSyntaxNode = GetDeclarationName(declarationSyntaxNode);
                    return (declarationSyntaxNode != null) && (ValidDeclarationNode(declarationSyntaxNode, nameSyntaxNode, position));
            }
        }

        private bool ValidDeclarationNode(SyntaxNode declarationSyntaxNode, IdentifierNameSyntax nameSyntaxNode, int position)
        {
            //check if previouls variable declaration has ";"
            if (!FirstOrClosedPrevVarDeclaration(declarationSyntaxNode))
                return false;
            return
                ((nameSyntaxNode == null) && (IsBeforeColonToken(declarationSyntaxNode, position))) ||
                ((nameSyntaxNode != null) && (nameSyntaxNode.Span.Start <= position) && (nameSyntaxNode.Span.End >= position));
        }

        private bool FirstOrClosedPrevVarDeclaration(SyntaxNode declarationSyntaxNode)
        {
            var containerSyntax = declarationSyntaxNode.FindParentByKind(ConvertedSyntaxKind.VarSection, ConvertedSyntaxKind.GlobalVarSection);
#if BC
            var varSection = containerSyntax as VarSectionBaseSyntax;
#else
            var varSection = containerSyntax as VarSectionSyntax;
#endif

            if (varSection != null)
            {
                int position = declarationSyntaxNode.FullSpan.Start - 1;
                SyntaxNode prevDeclaration = null;
                if (varSection.Variables != null)
                    for (int i = 0; (i < varSection.Variables.Count) && (varSection.Variables[i].FullSpan.Start <= position); i++)
                        prevDeclaration = varSection.Variables[i];

                if (prevDeclaration == null)
                    return true;

                if (ClosedVariableDeclarationSemicolon(prevDeclaration, true))
                    return true;

                return prevDeclaration.GetTrailingTrivia().HasNewLine();
            }
            return true;
        }

        private bool ClosedVariableDeclarationSemicolon(SyntaxNode node, bool defaultValue = false)
        {
            switch (node)
            {
                case VariableDeclarationSyntax prevVariableDeclaration:
                    return ((prevVariableDeclaration.SemicolonToken.Kind.ConvertToLocalType() == ConvertedSyntaxKind.SemicolonToken) && (prevVariableDeclaration.SemicolonToken.Span.Length > 0));
#if BC
                case VariableListDeclarationSyntax prevVariableListDeclaration:
                    return ((prevVariableListDeclaration.SemicolonToken.Kind.ConvertToLocalType() == ConvertedSyntaxKind.SemicolonToken) && (prevVariableListDeclaration.SemicolonToken.Span.Length > 0));
#endif
            }
            return defaultValue;
        }

        private ParameterSyntax FindParameterSyntax(ParameterListSyntax parameterList, int position)
        {
            if ((parameterList.CloseParenthesisToken.IsEmptyOrAfter(position)) &&
                (parameterList.Parameters != null) &&
                (parameterList.Parameters.Count > 0))
                for (int i = parameterList.Parameters.Count - 1; i >= 0; i--)
                    if (parameterList.Parameters[i].Span.Start < position)
                        return parameterList.Parameters[i];
            return null;
        }

        private bool IsBeforeColonToken(SyntaxNode node, int position)
        {
            if (node != null)
                switch (node)
                {
                    case VariableDeclarationSyntax variableDeclaration:
                        return 
                            (variableDeclaration.ColonToken.Kind == SyntaxKind.None) || 
                            (variableDeclaration.ColonToken.Span.Start >= position);
                    case ParameterSyntax parameter:
                        return
                            (parameter.ColonToken.Kind == SyntaxKind.None) ||
                            (parameter.ColonToken.Span.Start >= position);
                    case ReturnValueSyntax returnValue:
                        return
                            (returnValue.ColonToken.Kind == SyntaxKind.None) ||
                            (returnValue.ColonToken.Span.Start >= position);
                }
            return false;
        }

        private IdentifierNameSyntax GetDeclarationName(SyntaxNode node)
        {
            if (node != null)
                switch (node)
                {
                    case VariableDeclarationSyntax variableDeclaration:
                        return variableDeclaration.Name;
                    case ParameterSyntax parameter:
                        return parameter.Name;
                    case ReturnValueSyntax returnValue:
                        return returnValue.Name;
                }
            return null;
        }

        private void CreateCompletionItems(ALProject project, IEnumerable<ALAppObject> typesCollection, List<CodeCompletionItem> completionItems, bool asTemporaryVariable = false)
        {
            foreach (var type in typesCollection)
            {
                var varName = ALSyntaxHelper.ObjectNameToVariableNamePart(type.Name)
                    .RemovePrefixSuffix(project.MandatoryPrefixes, project.MandatorySuffixes, project.MandatoryAffixes);

                var addTemporary = (asTemporaryVariable) && (!varName.StartsWith(_tempPrefix, StringComparison.CurrentCultureIgnoreCase));

                if (addTemporary)
                    varName = _tempPrefix + varName;

                var source = varName;
                if (IncludeDataType)
                {
                    source = source + ": " +
                        type.GetALSymbolKind().ToVariableTypeName() + " " + ALSyntaxHelper.EncodeName(type.Name);
                    if (addTemporary)
                        source = source + " temporary";
                }
                var item = new CodeCompletionItem(source, CompletionItemKind.Field);
                item.filterText = varName;
                if (IncludeDataType)
                    item.commitCharacters = _fullDeclarationCommitCharacters;
                completionItems.Add(item);
            }
        }

    }
}
