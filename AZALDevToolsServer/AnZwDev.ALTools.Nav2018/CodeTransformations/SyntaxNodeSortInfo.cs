/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018.ALSymbols;
using AnZwDev.ALTools.Nav2018.ALSymbols.Internal;
using AnZwDev.ALTools.Nav2018.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.CodeTransformations
{
    public class SyntaxNodeSortInfo<T> where T: SyntaxNode
    {

        public string Name { get; set; }
        public int Index { get; set; }
        internal ConvertedSyntaxKind Kind { get; set; }
        public T Node { get; set; }

        public static List<SyntaxNodeSortInfo<T>> FromSyntaxList(SyntaxList<T> syntaxList)
        {
            List<SyntaxNodeSortInfo<T>> list = new List<SyntaxNodeSortInfo<T>>();
            for (int i=0; i<syntaxList.Count; i++)
            {
                list.Add(new SyntaxNodeSortInfo<T>(syntaxList[i], i));
            }
            return list;
        }

        public static SyntaxList<T> ToSyntaxList(List<SyntaxNodeSortInfo<T>> sortInfoList)
        {
            List<T> list = new List<T>();
            for (int i=0; i<sortInfoList.Count; i++)
            {
                list.Add(sortInfoList[i].Node);
            }
            return SyntaxFactory.List<T>(list);
        }

        public SyntaxNodeSortInfo()
        {
        }

        public SyntaxNodeSortInfo(T node, int index)
        {
            this.Index = index;
            this.Node = node;
            this.Kind = node.Kind.ConvertToLocalType();
            this.Name = this.Node.GetSyntaxNodeName();
        }

    }
}
