using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class SyntaxTreeExtensions
    {

        public static SyntaxTree SafeParseObjectText(string source)
        {
            SyntaxTree syntaxTree = null;

            try
            {
                syntaxTree = ParseObjectText(source);
            }
            catch (MissingMethodException e)
            {
                syntaxTree = ParseObjectTextNav2018(source);
            }

            return syntaxTree;
        }

        private static SyntaxTree ParseObjectText(string source)
        {
            return SyntaxTree.ParseObjectText(source);
        }

        private static SyntaxTree ParseObjectTextNav2018(string source)
        {
            return typeof(SyntaxTree).CallStaticMethod<SyntaxTree>("ParseObjectText", source,
                Type.Missing, Type.Missing, Type.Missing);
        }

    }
}
