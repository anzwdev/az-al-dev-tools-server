using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class IdentifierNameComparer : IComparer<IdentifierNameSyntax>
    {

        protected static AlphanumComparatorFast _stringComparer = new AlphanumComparatorFast();

        public int Compare(IdentifierNameSyntax x, IdentifierNameSyntax y)
        {
            string xName = x.Identifier.ValueText;
            string yName = y.Identifier.ValueText;
            return _stringComparer.Compare(xName, yName);
        }
    }
}
