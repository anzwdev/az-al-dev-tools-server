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

        public static List<SyntaxTrivia> RemoveEmptyLines(this IEnumerable<SyntaxTrivia> triviaList)
        {
            List<SyntaxTrivia> newList = new List<SyntaxTrivia>();
            bool validLine = true;
            int lastValidPos = -1;

            foreach (SyntaxTrivia trivia in triviaList)
            {
                //add trivia to the list
                newList.Add(trivia);
                
                ConvertedSyntaxKind kind = trivia.Kind.ConvertToLocalType();

                if (kind == ConvertedSyntaxKind.EndOfLineTrivia)
                {
                    //if valid line, then remember this trivia as last valid position
                    //if invalid line, remove elements after last valid position
                    if (validLine)
                        lastValidPos = newList.Count - 1;
                    else
                        newList.RemoveRange(lastValidPos + 1, newList.Count - lastValidPos - 1);
                    validLine = false;
                } 
                else if (kind != ConvertedSyntaxKind.WhiteSpaceTrivia)
                    validLine = true;

                if (validLine)
                    lastValidPos = newList.Count - 1;
            }

            return newList;
        }

    }
}
