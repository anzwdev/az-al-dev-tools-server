using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeCompletion
{
    public class CodeCompletionProvidersCollection
    {

        private Dictionary<string, CodeCompletionProvider> _completionProviders = new Dictionary<string, CodeCompletionProvider>();

        public CodeCompletionProvidersCollection()
        {
            this.Add(new VariableNamesCompletionProvider(true));
            this.Add(new VariableNamesCompletionProvider(false));
            this.Add(new VariableDataTypesCompletionProvider());
        }

        private void Add(CodeCompletionProvider provider)
        {
            _completionProviders.Add(provider.Name, provider);
        }

        public void CollectCompletionItems(ALWorkspace workspace, string filePath, Position linePosition, List<string> providers, List<CodeCompletionItem> completionItems)
        {
            if ((workspace.ActiveDocument.Project != null) && (workspace.ActiveDocument.SyntaxTree != null))
                CollectCompletionItems(workspace.ActiveDocument.Project, workspace.ActiveDocument.SyntaxTree, linePosition, providers, completionItems);
        }

        private void CollectCompletionItems(ALProject project, SyntaxTree syntaxTree, Position linePosition, List<string> providers, List<CodeCompletionItem> completionItems)
        {
            if ((providers != null) && (providers.Count > 0))
            {

                var position = syntaxTree.GetText().Lines.GetPosition(new LinePosition(linePosition.line, linePosition.character));
                SyntaxNode syntaxNode = syntaxTree.FindNodeByPositionInFullSpan(position);

                if (syntaxNode != null)
                {
                    HashSet<string> providersNamesSet = providers.ToHashSet<string>(false);
                    foreach (var providerName in providersNamesSet)
                    {
                        if (_completionProviders.ContainsKey(providerName))
                        {
                            var provider = _completionProviders[providerName];
                            if (provider.CanBeUsed(providersNamesSet))
                                provider.CollectCompletionItems(project, syntaxTree, syntaxNode, position, completionItems);
                        }
                    }
                }
            }
        }

    }
}
