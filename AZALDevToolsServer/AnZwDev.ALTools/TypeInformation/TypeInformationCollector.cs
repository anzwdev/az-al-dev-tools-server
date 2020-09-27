using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnZwDev.ALTools.TypeInformation
{
    public class TypeInformationCollector
    {

        public ProjectTypesInformation ProjectTypesInformation { get; }

        public TypeInformationCollector()
        {
            this.ProjectTypesInformation = new ProjectTypesInformation();
        }

        public void Clear()
        {
            this.ProjectTypesInformation.Clear();
        }

        public void Visit(SyntaxNode node)
        {
            if (VisitNode(node))
            {
                foreach (SyntaxNode childNode in node.ChildNodes())
                {
                    Visit(childNode);
                }
            }
        }

        protected bool VisitNode(SyntaxNode node)
        {
            ConvertedSyntaxKind kind = node.Kind.ConvertToLocalType();
            switch (kind)
            {
                case ConvertedSyntaxKind.TableObject:
                    this.ProjectTypesInformation.Add(new TableTypeInformation((TableSyntax)node));
                    break;
                case ConvertedSyntaxKind.CompilationUnit:
                    return true;
            }
            return false;
        }

        public void VisitFile(string fileName)
        {
            if (!String.IsNullOrWhiteSpace(fileName))
                this.VisitSourceCode(File.ReadAllText(fileName));
        }

        public void VisitSourceCode(string sourceCode)
        {
            SyntaxTree syntaxTree = SyntaxTreeExtensions.SafeParseObjectText(sourceCode);
            if (syntaxTree != null)
            {
                SyntaxNode node = syntaxTree.GetRoot();
                if (node != null)
                    this.Visit(node);
            }
        }

        public void VisitDirectory(string path)
        {
            string[] filePathsList = System.IO.Directory.GetFiles(path, "*.al", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < filePathsList.Length; i++)
            {
                this.VisitFile(filePathsList[i]);
            }
        }

    }
}
