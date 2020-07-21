/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using AnZwDev.ALTools.Nav2018.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Packaging;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.CodeTransformations
{
    public class ALSyntaxRewriter : SyntaxRewriter
    {

        public int NoOfChanges { get; set; }
        public int TotalNoOfChanges { get; set; }
        public int NoOfChangedFiles { get; set; }
        

        public ALSyntaxRewriter()
        {
            this.NoOfChanges = 0;
            this.NoOfChangedFiles = 0;
            this.TotalNoOfChanges = 0;
        }

        public virtual string RewriteSourceCode(string source)
        {
            this.NoOfChanges = 0;
            BeforeVisitSourceCode();

            //parse source code
            SyntaxTree syntaxTree = SyntaxTreeExtensions.SafeParseObjectText(source);

            //fix nodes
            SyntaxNode node = this.Visit(syntaxTree.GetRoot());
            node = this.AfterVisitSourceCode(node);

            //return new source code
            if (node == null)
                return null;

            //update statistics only if node exists
            this.TotalNoOfChanges += this.NoOfChanges;
            if (this.NoOfChanges > 0)
                this.NoOfChangedFiles++;

            return node.ToFullString();
        }

        public bool RewriteFile(string filePath)
        {
            try
            {
                string source = System.IO.File.ReadAllText(filePath);
                string newSource = this.RewriteSourceCode(source);
                if ((newSource != source) && (this.NoOfChanges > 0) && (!String.IsNullOrWhiteSpace(newSource)))
                    System.IO.File.WriteAllText(filePath, newSource);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public virtual void RewriteDirectory(string directoryPath)
        {
            string[] filePathsList = System.IO.Directory.GetFiles(directoryPath, "*.al", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < filePathsList.Length; i++)
            {
                this.RewriteFile(filePathsList[i]);
            }
        }

        protected virtual void BeforeVisitSourceCode()
        {
        }

        protected virtual SyntaxNode AfterVisitSourceCode(SyntaxNode node)
        {
            return node;
        }

    }
}
