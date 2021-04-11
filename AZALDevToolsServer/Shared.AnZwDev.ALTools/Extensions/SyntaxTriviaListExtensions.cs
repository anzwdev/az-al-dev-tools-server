using AnZwDev.ALTools.ALSymbols.Internal;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class SyntaxTriviaListExtensions
    {

        public static SyntaxTriviaList NormalizeSyntaxTriviaList(this SyntaxTriviaList triviaList)
        {
            List<SyntaxTrivia> newList = new List<SyntaxTrivia>();

            for (int triviaIdx = 0; triviaIdx < triviaList.Count; triviaIdx++)
            {
                SyntaxTrivia trivia = triviaList[triviaIdx];
                bool addTrivia = true;
                switch (trivia.Kind.ConvertToLocalType())
                {
                    case ConvertedSyntaxKind.WhiteSpaceTrivia:
                        addTrivia = (triviaIdx == (triviaList.Count - 1)) ||
                            (triviaList[triviaIdx + 1].Kind.ConvertToLocalType() != ConvertedSyntaxKind.EndOfLineTrivia);
                        break;
                    case ConvertedSyntaxKind.EndOfLineTrivia:
                        addTrivia = (newList.Count == 0) ||
                            (newList[newList.Count - 1].Kind.ConvertToLocalType() != ConvertedSyntaxKind.EndOfLineTrivia);
                        break;
                }
                if (addTrivia)
                    newList.Add(triviaList[triviaIdx]);
            }
            return SyntaxFactory.TriviaList(newList);
        }

    }
}
