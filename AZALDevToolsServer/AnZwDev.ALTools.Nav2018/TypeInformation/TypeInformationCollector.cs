/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018.ALSymbols.Internal;
using AnZwDev.ALTools.Nav2018.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.TypeInformation
{
    public class TypeInformationCollector: SyntaxWalker
    {

        public ProjectTypesInformation ProjectTypesInformation { get; }

        public TypeInformationCollector()
        {
            this.ProjectTypesInformation = new ProjectTypesInformation();
        }

        public override void Visit(SyntaxNode node)
        {
            ConvertedSyntaxKind kind = node.Kind.ConvertToLocalType();

            switch (kind)
            {
                case ConvertedSyntaxKind.TableObject:
                    this.ProjectTypesInformation.Add(new TableTypeInformation((TableSyntax)node));
                    break;
                case ConvertedSyntaxKind.PageObject:
                case ConvertedSyntaxKind.PageExtensionObject:
                case ConvertedSyntaxKind.TableExtensionObject:
                case ConvertedSyntaxKind.ReportObject:
                case ConvertedSyntaxKind.XmlPortObject:
                case ConvertedSyntaxKind.CodeunitObject:
                case ConvertedSyntaxKind.EnumType:
                case ConvertedSyntaxKind.EnumExtensionType:
                case ConvertedSyntaxKind.ProfileObject:
                case ConvertedSyntaxKind.PageCustomizationObject:
                case ConvertedSyntaxKind.Interface:
                    break;
                default:
                    base.Visit(node);
                    break;
            }
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
                SyntaxNode node = syntaxTree.GetCompilationUnitRoot();
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
