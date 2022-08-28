using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RemoveEmptyTriggersSyntaxRewriter : ALSyntaxRewriter
    {

        public bool RemoveTriggers { get; set; }
        public bool RemoveSubscribers { get; set; }

        public RemoveEmptyTriggersSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitTriggerDeclaration(TriggerDeclarationSyntax node)
        {
            if (RemoveTriggers && IsEmptyMethod(node))
            {
                NoOfChanges++;
                return null;
            }
            return base.VisitTriggerDeclaration(node);
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (RemoveSubscribers && IsEmptyMethod(node) && IsEventSubscriber(node))
            {
                NoOfChanges++;
                return null;
            }
            return base.VisitMethodDeclaration(node);
        }

        private bool IsEmptyMethod(MethodOrTriggerDeclarationSyntax syntax)
        {
            return
                (syntax.Body?.Statements == null) ||
                (syntax.Body.Statements.Count == 0);
        }

        private bool IsEventSubscriber(MethodDeclarationSyntax syntax)
        {
            return
                (syntax.Attributes != null) &&
                (syntax.Attributes.Where(p => (p.Name != null) && (p.Name.ToString().Equals("EventSubscriber", StringComparison.CurrentCultureIgnoreCase))).Any());
        }
    }
}
