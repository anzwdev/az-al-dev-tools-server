using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.TypeInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class BasePageWithSourceSyntaxRewriter : ALSyntaxRewriter
    {

        public TypeInformationCollector TypeInformationCollector { get; }
        public TableTypeInformation CurrentTable { get; set; }

        public BasePageWithSourceSyntaxRewriter()
        {
            this.TypeInformationCollector = new TypeInformationCollector();
        }

        public override SyntaxNode VisitRequestPage(RequestPageSyntax node)
        {
            this.CheckSourceTableProperty(node);
            return base.VisitRequestPage(node);
        }

        public override SyntaxNode VisitPageExtension(PageExtensionSyntax node)
        {
            this.CurrentTable = null;
            return base.VisitPageExtension(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            this.CheckSourceTableProperty(node);
            return base.VisitPage(node);
        }

        protected void CheckSourceTableProperty(SyntaxNode node)
        {
            //try to find current table
            this.CurrentTable = null;
            PropertyValueSyntax sourceTablePropertyValue = node.GetPropertyValue("SourceTable");
            if (sourceTablePropertyValue != null)
            {
                string sourceTable = ALSyntaxHelper.DecodeName(sourceTablePropertyValue.ToString());
                this.CurrentTable = this.TypeInformationCollector.ProjectTypesInformation.GetTable(sourceTable);
            }
        }

        protected string GetFieldCaption(PageFieldSyntax node)
        {
            //try to find source field caption
            string caption = null;
            if (node.Expression != null)
            {
                string source = node.Expression.ToString().Trim();
                if (source.StartsWith("Rec.", StringComparison.CurrentCultureIgnoreCase))
                    source = source.Substring(4).Trim();
                source = ALSyntaxHelper.DecodeName(source);
                if (!String.IsNullOrWhiteSpace(source))
                {
                    if (this.CurrentTable != null)
                    {
                        TableFieldTypeInformation tableField = this.CurrentTable.GetField(source);
                        if ((tableField != null) && (!String.IsNullOrWhiteSpace(tableField.Caption)))
                            caption = tableField.Caption;
                    }
                    if (String.IsNullOrWhiteSpace(caption))
                        caption = source.Replace("\"", "");
                }
            }
            return caption;
        }

    }
}
