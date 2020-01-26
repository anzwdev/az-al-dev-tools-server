using AnZwDev.ALTools.ALProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class AppAreaManager
    {
        public ALExtensionProxy ALExtensionProxy { get; }

        protected ALExtensionLibraryTypeProxy _separatedIdentifierNameList;
        protected ALExtensionLibraryTypeProxy _syntaxFactoryProxy;
        protected ALExtensionLibraryTypeProxy _syntaxNodeExtensionsProxy;

        public AppAreaManager(ALExtensionProxy proxy)
        {
            this.ALExtensionProxy = proxy;
            //create reflection proxy classes
            this._separatedIdentifierNameList = new ALExtensionLibraryTypeProxy(
                this.ALExtensionProxy.CodeAnalysis,
                "Microsoft.Dynamics.Nav.CodeAnalysis.Syntax.SeparatedSyntaxList`1",
                "Microsoft.Dynamics.Nav.CodeAnalysis.Syntax.IdentifierNameSyntax"
                );
            this._syntaxFactoryProxy = new ALExtensionLibraryTypeProxy(
                this.ALExtensionProxy.CodeAnalysis,
                "Microsoft.Dynamics.Nav.CodeAnalysis.SyntaxFactory");
            this._syntaxNodeExtensionsProxy = new ALExtensionLibraryTypeProxy(
                this.ALExtensionProxy.CodeAnalysis,
                "Microsoft.Dynamics.Nav.CodeAnalysis.Syntax.SyntaxNodeExtensions");
        }

        public string AddMissingAppAreas(string source, string newAppAreaName, out int noOfAppAreas)
        {
            noOfAppAreas = 0;
            //parse source code
            dynamic syntaxTree = this.ALExtensionProxy.GetSyntaxTree(source);
            dynamic node = syntaxTree.GetRoot();

            //fix nodes
            node = this.AddMissingAppAreasToRootNode(node, newAppAreaName, out noOfAppAreas);

            //return new source code
            if ((node != null) && (noOfAppAreas > 0))
                return node.ToFullString();

            return null;
        }

        protected dynamic AddMissingAppAreasToRootNode(dynamic node, string newAppArea, out int noOfNodes)
        {
            noOfNodes = 0;
            dynamic nodeToFix = null;
            do
            {
                nodeToFix = FindFirstNodeToFix(node);
                if (nodeToFix != null)
                {
                    node = _syntaxNodeExtensionsProxy.CallStaticMethod("ReplaceNode", node,
                        nodeToFix, AddAppAreasToNode(nodeToFix, newAppArea));
                    noOfNodes++;
                }
            } while (nodeToFix != null);

            return node;
        }

        protected dynamic AddAppAreasToNode(dynamic original, string newAppArea)
        {
            dynamic list = _separatedIdentifierNameList.CreateInstance();

            list = list.Add(_syntaxFactoryProxy.CallStaticMethod("IdentifierName", newAppArea));

            var property = _syntaxFactoryProxy.CallStaticMethod("Property", "ApplicationArea",
                _syntaxFactoryProxy.CallStaticMethod("CommaSeparatedPropertyValue", list));

            int indentLength = 4;
            string indent;
            dynamic leadingTrivia = original.GetLeadingTrivia();
            if (leadingTrivia != null)
            {
                indent = leadingTrivia.ToString();
                int newLinePos = indent.LastIndexOf("/n");
                if (newLinePos >= 0)
                    indent = indent.Substring(newLinePos + 1);
                indentLength += indent.Length;
            }
            indent = "".PadLeft(indentLength);

            dynamic leadingTriviaList = _syntaxFactoryProxy.CallStaticMethod("ParseLeadingTrivia", indent, 0);
            dynamic trailingTriviaList = _syntaxFactoryProxy.CallStaticMethod("ParseTrailingTrivia", "\r\n", 0);

            property = _syntaxNodeExtensionsProxy.CallStaticMethod("WithLeadingTrivia",
                property, leadingTriviaList);

            property = _syntaxNodeExtensionsProxy.CallStaticMethod("WithTrailingTrivia",
                property, trailingTriviaList);

            return original.AddPropertyListProperties(property);
        }

        public dynamic FindFirstNodeToFix(dynamic node)
        {
            bool skipChildren;
            if (IsAppAreaMissing(node, out skipChildren))
                return node;

            if (!skipChildren)
            {
                IEnumerable<dynamic> childNodeList = node.ChildNodes();
                if (childNodeList != null)
                {
                    foreach (dynamic childNode in childNodeList)
                    {
                        dynamic outVal = this.FindFirstNodeToFix(childNode);
                        if (outVal != null)
                            return outVal;
                    }
                }
            }
            return null;
        }

        protected bool IsAppAreaMissing(dynamic node, out bool skipChildren)
        {
            skipChildren = false;

            switch (node.Kind.ToString())
            {
                case "PageField":
                case "PageUserControl":
                case "PagePart":
                case "PageSystemPart":
                case "PageChartPart":
                case "PageAction":
                    skipChildren = true;
                    dynamic property = this._syntaxNodeExtensionsProxy.CallStaticMethod(
                        "GetProperty", node, "ApplicationArea");
                    if ((property == null) || (String.IsNullOrWhiteSpace(property.Value.ToString())))
                        return true;
                    break;
            }
            return false;
        }


    }
}
