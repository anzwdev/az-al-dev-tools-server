using Microsoft.Dynamics.Nav.CodeAnalysis;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.DuplicateCodeSearch
{
    public class DCStatementKeyDictionaryBuilder : SyntaxRewriter
    {

        protected string _sourceFilePath;
        public DCStatementKeyDictionary StatementsDictionary { get; } = new DCStatementKeyDictionary();

        public DCStatementKeyDictionaryBuilder()
        {
        }

        public void VisitProjectFolder(string filePath)
        {
            string[] files = Directory.GetFiles(filePath, "*.al", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
                VisitFile(files[i]);
        }

        public void VisitFile(string filePath)
        {
            _sourceFilePath = filePath;
            SyntaxTree syntaxTree = SyntaxTree.ParseObjectText(File.ReadAllText(_sourceFilePath));
            Visit(syntaxTree.GetRoot());
        }

        public override SyntaxNode VisitTriggerDeclaration(TriggerDeclarationSyntax node)
        {
            VisitMethodOrTriggerDeclaration(node);
            return base.VisitTriggerDeclaration(node);
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            VisitMethodOrTriggerDeclaration(node);
            return base.VisitMethodDeclaration(node);
        }

        protected void VisitMethodOrTriggerDeclaration(MethodOrTriggerDeclarationSyntax node)
        {
            if (node.Body != null)
            {
                DCStatementsBlock statementsBlock = new DCStatementsBlock(_sourceFilePath);
                AppendStatement(node.Body, statementsBlock);
            }
        }

        protected void AppendStatement(StatementSyntax node, DCStatementsBlock statementsBlock)
        {
            switch (node)
            {
                case BlockSyntax blockSyntax:
                    AppendBlockStatement(blockSyntax, statementsBlock);
                    break;
                case RepeatStatementSyntax repeatStatementSyntax:
                    AppendRepeatStatement(repeatStatementSyntax, statementsBlock);
                    break;
                case WhileStatementSyntax whileStatementSyntax:
                    AppendWhileStatement(whileStatementSyntax, statementsBlock);
                    break;
                case ForStatementSyntax forStatementSyntax:
                    AppendForStatement(forStatementSyntax, statementsBlock);
                    break;
                case IfStatementSyntax ifStatementSyntax:
                    AppendIfStatement(ifStatementSyntax, statementsBlock);
                    break;
                case CaseStatementSyntax caseStatementSyntax:
                    AppendCaseStatement(caseStatementSyntax, statementsBlock);
                    break;
                case WithStatementSyntax withStatementSyntax:
                    AppendWithStatement(withStatementSyntax, statementsBlock);
                    break;
                default:
                    AppendFullStatement(node, statementsBlock);
                    break;
            }
        }

        protected void AppendBlockStatement(BlockSyntax blockSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(blockSyntax.BeginKeywordToken, statementsBlock);
            foreach (StatementSyntax statementSyntax in blockSyntax.Statements)
                AppendStatement(statementSyntax, statementsBlock);
            AppendToken(blockSyntax.EndKeywordToken, statementsBlock);
        }

        protected void AppendRepeatStatement(RepeatStatementSyntax repeatStatementSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(repeatStatementSyntax.RepeatKeywordToken, statementsBlock);
            foreach (StatementSyntax statementSyntax in repeatStatementSyntax.Statements)
                AppendStatement(statementSyntax, statementsBlock);
            AppendToken(repeatStatementSyntax.UntilKeywordToken, statementsBlock);
            AppendExpression(repeatStatementSyntax.Condition, statementsBlock);
        }

        protected void AppendWhileStatement(WhileStatementSyntax whileStatementSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(whileStatementSyntax.WhileKeywordToken, statementsBlock);
            AppendExpression(whileStatementSyntax.Condition, statementsBlock);
            AppendToken(whileStatementSyntax.DoKeywordToken, statementsBlock);
            AppendStatement(whileStatementSyntax.Statement, statementsBlock);
        }

        protected void AppendForStatement(ForStatementSyntax forStatementSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(forStatementSyntax.ForKeywordToken, statementsBlock);
            AppendExpression(forStatementSyntax.LoopVariable, statementsBlock);
            AppendToken(forStatementSyntax.AssignToken, statementsBlock);
            AppendExpression(forStatementSyntax.InitialValue, statementsBlock);
            AppendToken(forStatementSyntax.OperatorKeywordToken, statementsBlock);
            AppendExpression(forStatementSyntax.EndValue, statementsBlock);
            AppendToken(forStatementSyntax.DoKeywordToken, statementsBlock);
            AppendStatement(forStatementSyntax.Statement, statementsBlock);
        }

        protected void AppendIfStatement(IfStatementSyntax ifStatementSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(ifStatementSyntax.IfKeywordToken, statementsBlock);
            AppendExpression(ifStatementSyntax.Condition, statementsBlock);
            AppendToken(ifStatementSyntax.ThenKeywordToken, statementsBlock);
            AppendStatement(ifStatementSyntax.Statement, statementsBlock);
            if ((ifStatementSyntax.ElseKeywordToken != null) && (ifStatementSyntax.ElseKeywordToken.Kind.ConvertToLocalType() == ALSymbols.Internal.ConvertedSyntaxKind.ElseKeyword))
                AppendToken(ifStatementSyntax.ElseKeywordToken, statementsBlock);
            if (ifStatementSyntax.ElseStatement != null)
                AppendStatement(ifStatementSyntax.ElseStatement, statementsBlock);
        }

        protected void AppendCaseStatement(CaseStatementSyntax caseStatementSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(caseStatementSyntax.CaseKeywordToken, statementsBlock);
            AppendExpression(caseStatementSyntax.Expression, statementsBlock);
            AppendToken(caseStatementSyntax.OfKeywordToken, statementsBlock);
            foreach (CaseLineSyntax caseLineSyntax in caseStatementSyntax.CaseLines)
                AppendCaseLineStatement(caseLineSyntax, statementsBlock);
            if (caseStatementSyntax.CaseElse != null)
                AppendCaseElseStatement(caseStatementSyntax.CaseElse, statementsBlock);
            AppendToken(caseStatementSyntax.EndKeywordToken, statementsBlock);
        }

        protected void AppendCaseLineStatement(CaseLineSyntax caseLineSyntax, DCStatementsBlock statementsBlock)
        {
            foreach (CodeExpressionSyntax codeExpressionSyntax in caseLineSyntax.Expressions)
                AppendExpression(codeExpressionSyntax, statementsBlock);
            AppendStatement(caseLineSyntax.Statement, statementsBlock);
        }

        protected void AppendCaseElseStatement(CaseElseSyntax caseElseSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(caseElseSyntax.ElseKeywordToken, statementsBlock);
            foreach (StatementSyntax statementSyntax in caseElseSyntax.ElseStatements)
                AppendStatement(statementSyntax, statementsBlock);
        }

        protected void AppendWithStatement(WithStatementSyntax withStatementSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(withStatementSyntax.WithKeywordToken, statementsBlock);
            AppendExpression(withStatementSyntax.WithId, statementsBlock);
            AppendToken(withStatementSyntax.DoKeywordToken, statementsBlock);
            AppendStatement(withStatementSyntax.Statement, statementsBlock);
        }

        protected void AppendFullStatement(StatementSyntax statementSyntax, DCStatementsBlock statementsBlock)
        {
            AppendFullNode(statementSyntax, statementsBlock);
        }

        protected void AppendExpression(ExpressionSyntax expressionSyntax, DCStatementsBlock statementsBlock)
        {
            AppendFullNode(expressionSyntax, statementsBlock);
        }

        protected void AppendToken(SyntaxToken token, DCStatementsBlock statementsBlock)
        {
            AppendStatementInstance(
                token.ToString().ToLower(),
                true,
                new Range(token.SyntaxTree.GetLineSpan(token.Span)), 
                statementsBlock);
        }

        protected void AppendFullNode(SyntaxNode node, DCStatementsBlock statementsBlock)
        {
            StringBuilder keyBuilder = new StringBuilder();
            foreach (SyntaxToken token in node.DescendantTokens())
            {
                keyBuilder.Append(token.ToString());
            }

            AppendStatementInstance(
                keyBuilder.ToString().ToLower(), 
                false,
                new Range(node.SyntaxTree.GetLineSpan(node.Span)), 
                statementsBlock);
        }

        protected void AppendStatementInstance(string keyValue, bool ignore, Range range, DCStatementsBlock statementsBlock)
        {
            DCStatementKey key = this.StatementsDictionary.GetOrCreate(keyValue, ignore);
            DCStatementInstance statementInstance = new DCStatementInstance(statementsBlock, key, range, statementsBlock.Statements.Count);
            statementsBlock.Statements.Add(statementInstance);
            key.StatementInstances.Add(statementInstance);
        }

    }
}
