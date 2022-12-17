using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Extensions.FileSystemGlobbing;
using AnZwDev.ALTools.ALLanguageInformation;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class AddTableDataCaptionsRewriter : ALSyntaxRewriter
    {

        public List<string> FieldsPatterns { get; set; } = null;

        public AddTableDataCaptionsRewriter()
        {
        }

        public override SyntaxNode VisitTable(TableSyntax node)
        {
            if (node.GetProperty("DataCaptionFields") == null)
            {
                var tableName = ALSyntaxHelper.DecodeName(node.Name?.ToString());

                if (tableName != null)
                {
                    var matcher = new TableFieldsInformationPatternMatcher();
                    var collectedFields = matcher.Match(Project, tableName, true, FieldsPatterns, true, true, false);

                    if (collectedFields.Count > 0)
                    {
                        var fieldNamesSyntaxList = new List<IdentifierNameSyntax>();
                        for (int i = 0; i < collectedFields.Count; i++)
                            fieldNamesSyntaxList.Add(SyntaxFactory.IdentifierName(collectedFields[i].Name));

                        var fieldNamesSyntaxSeparatedList = new SeparatedSyntaxList<IdentifierNameSyntax>();
                        fieldNamesSyntaxSeparatedList = fieldNamesSyntaxSeparatedList.AddRange(fieldNamesSyntaxList);
                        var propertyValue = SyntaxFactory.CommaSeparatedPropertyValue(fieldNamesSyntaxSeparatedList);
                        var property = SyntaxFactory.Property("DataCaptionFields", propertyValue);

                        node = node.WithPropertyList(node.PropertyList.AddProperties(property));
                    }
                }
            }

            return base.VisitTable(node);
        }

    }
}
