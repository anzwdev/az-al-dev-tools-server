using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class DirectiveTriviaMergedList
    {

        public List<SyntaxTrivia> List { get; }
        private bool _addNewLine = false;

        public DirectiveTriviaMergedList()
        {
            List = new List<SyntaxTrivia>();
        }

        public void AddRange(IEnumerable<SyntaxTrivia> collection)
        {
            foreach (var trivia in collection)
            {
                var kind = trivia.Kind.ConvertToLocalType();
                if (kind == ConvertedSyntaxKind.EndOfLineTrivia)
                {
                    if (_addNewLine)
                        List.Add(trivia);
                    _addNewLine = false;
                } 
                else if (CanMerge(kind))
                {
                    List.Add(trivia);
                    _addNewLine = true;
                }
            }
        }

        private bool CanMerge(ConvertedSyntaxKind kind)
        {
            switch (kind)
            {
                case ConvertedSyntaxKind.BadDirectiveTrivia:
                case ConvertedSyntaxKind.BadPragmaDirectiveTrivia:
                case ConvertedSyntaxKind.DefineDirectiveTrivia:
                case ConvertedSyntaxKind.ElifDirectiveTrivia:
                case ConvertedSyntaxKind.ElseDirectiveTrivia:
                case ConvertedSyntaxKind.IfDirectiveTrivia:
                case ConvertedSyntaxKind.PreprocessingMessageTrivia:
                case ConvertedSyntaxKind.RegionDirectiveTrivia:
                case ConvertedSyntaxKind.UndefDirectiveTrivia:
                case ConvertedSyntaxKind.PragmaImplicitWithDirectiveTrivia:
                case ConvertedSyntaxKind.PragmaWarningDirectiveTrivia:
                    return true;
                case ConvertedSyntaxKind.EndRegionDirectiveTrivia:
                    return !RemoveLastRegion();
            }
            return false;
        }

        private bool RemoveLastRegion()
        {
            for (int i=List.Count - 1; i>=0; i++)
                if (List[i].Kind.ConvertToLocalType() == ConvertedSyntaxKind.RegionDirectiveTrivia)
                {
                    List.RemoveAt(i);
                    return true;
                }
            return false;
        }

    }

}
