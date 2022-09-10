using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeCompletion
{
    public class CodeCompletionProvider
    {

        public string Name { get; }
        public CodeCompletionProvider(string name)
        {
            Name = name;
        }

        public virtual void CollectCompletionItems(ALProject project, SyntaxTree syntaxTree, SyntaxNode syntaxNode, int position, List<CodeCompletionItem> completionItems)
        {
        }

        public virtual bool CanBeUsed(HashSet<string> providerNames)
        {
            return providerNames.Contains(Name);
        }

    }
}
