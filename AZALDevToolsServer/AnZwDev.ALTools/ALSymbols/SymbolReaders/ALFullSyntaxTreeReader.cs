using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.SymbolReaders
{
    public class ALFullSyntaxTreeReader
    {

        public ALFullSyntaxTreeReader()
        {
        }

        #region Main processing methods

        public ALFullSyntaxTreeNode ProcessSourceFile(string fileName)
        {
            string sourceCode;
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(fileName);
                sourceCode = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();

                return ProcessSourceCode(sourceCode);
            }
            catch (Exception e)
            {
                return new ALFullSyntaxTreeNode(e);
            }

        }

        public ALFullSyntaxTreeNode ProcessSourceCode(string source)
        {
            try
            {
                SyntaxTree sourceTree = SyntaxTree.ParseObjectText(source);
                return ProcessSyntaxTree(sourceTree);
            }
            catch (Exception e)
            {
                return new ALFullSyntaxTreeNode(e);
            }
        }

        public ALFullSyntaxTreeNode ProcessSyntaxTree(SyntaxTree syntaxTree)
        {
            SyntaxNode node = syntaxTree.GetRoot();
            return ProcessSyntaxTreeNode(syntaxTree, node);
        }

        #endregion

        #region Processing nodes

        protected ALFullSyntaxTreeNode ProcessSyntaxTreeNode(SyntaxTree syntaxTree, SyntaxNode node)
        {
            //process node
            ALFullSyntaxTreeNode alNode = CreateALNode(syntaxTree, node);
            if (alNode == null)
                return null;

            //process child nodes
            IEnumerable<SyntaxNode> list = node.ChildNodes();
            if (list != null)
            {
                foreach (SyntaxNode childNode in list)
                {
                    ALFullSyntaxTreeNode childALNode = ProcessSyntaxTreeNode(syntaxTree, childNode);
                    if (childALNode != null)
                        alNode.AddChildNode(childALNode);
                }
            }

            return alNode;
        }

        protected ALFullSyntaxTreeNode CreateALNode(SyntaxTree syntaxTree, SyntaxNode node)
        {
            return null;
            /*

            //base syntax node properties
            ALFullSyntaxTreeNode alNode = new ALFullSyntaxTreeNode();
            alNode.kind = node.Kind.ToString();

            dynamic lineSpan = syntaxTree.GetLineSpan(node.FullSpan);
            alNode.fullSpan = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);

            lineSpan = syntaxTree.GetLineSpan(node.Span);
            alNode.span = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character);

            //additional properties
            Type nodeType = node.GetType();

            if ((nodeType.GetProperty("Name") != null) && (node.Name != null))
                alNode.name = ALSyntaxHelper.DecodeName(node.Name.ToString());

            if (nodeType.GetProperty("Attributes") != null)
            {
                IEnumerable<dynamic> att = node.Attributes;
                foreach (dynamic childNode in att)
                {
                    alNode.AddAttribute(CreateALNode(syntaxTree, childNode));
                }
            }

            if ((nodeType.GetProperty("OpenBraceToken") != null) && (node.OpenBraceToken != null))
                alNode.openBraceToken = CreateALNode(syntaxTree, node.OpenBraceToken);

            if ((nodeType.GetProperty("CloseBraceToken") != null) && (node.CloseBraceToken != null))
                alNode.closeBraceToken = CreateALNode(syntaxTree, node.CloseBraceToken);

            if ((nodeType.GetProperty("VarKeyword") != null) && (node.VarKeyword != null))
                alNode.varKeyword = CreateALNode(syntaxTree, node.VarKeyword);

            alNode.accessModifier = this.GetStringProperty(node, nodeType, "AccessModifier");
            alNode.identifier = this.GetStringProperty(node, nodeType, "Identifier");
            alNode.dataType = this.GetStringProperty(node, nodeType, "DataType");
            alNode.temporary = this.GetStringProperty(node, nodeType, "Temporary");

            return alNode;
            */
        }


        #endregion

    }
}
