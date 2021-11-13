using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{

#if BC

    public class AddParenthesesSyntaxRewriter : ALSemanticModelSyntaxRewriter
    {

        public AddParenthesesSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                (SyntaxNode newNode, bool updated) = this.UpdateSyntaxNode(node);
                if (updated)
                    return newNode;
            }

            return base.VisitIdentifierName(node);
        }

        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                (SyntaxNode newNode, bool updated) = this.UpdateSyntaxNode(node);
                if (updated)
                    return newNode;
            }

            return base.VisitMemberAccessExpression(node);
        }

        protected (SyntaxNode, bool) UpdateSyntaxNode(CodeExpressionSyntax node)
        {
            if (this.SemanticModel.GetOperation(node) is IInvocationExpression operation)
            {
                IMethodSymbol targetMethod = operation.TargetMethod;
                ConvertedMethodKind targetMethodKind = (targetMethod == null) ? ConvertedMethodKind.Method : targetMethod.MethodKind.ConvertToLocalType();

                if ((targetMethod == null) || (targetMethodKind != ConvertedMethodKind.Property))
                {
                    bool isBuiltInProperty = false;
                    if ((targetMethod != null) && (targetMethodKind == ConvertedMethodKind.BuiltInMethod) && (targetMethod is IBuiltInMethodTypeSymbol builtInMethodTypeSymbol))
                        isBuiltInProperty = builtInMethodTypeSymbol.IsProperty;

                    SyntaxToken lastToken = operation.Syntax.GetLastToken();
                    bool hasCloseParenToken = ((lastToken != null) && (lastToken.Kind.ConvertToLocalType() == ConvertedSyntaxKind.CloseParenToken));

                    if (
                        ((targetMethod == null) || (!isBuiltInProperty)) &&
                        (!hasCloseParenToken))
                    {
                        SyntaxNode operationNode = operation.Syntax;
                        if (operationNode == node)
                            return (SyntaxFactory.InvocationExpression(node).WithTriviaFrom(node), true);
                    }
                }
            }
            return (node, false);
        }


        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            return base.VisitInvocationExpression(node);
        }


    }

#endif

}
