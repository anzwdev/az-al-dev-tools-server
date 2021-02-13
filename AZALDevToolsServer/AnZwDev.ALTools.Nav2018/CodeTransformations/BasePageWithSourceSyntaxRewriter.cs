using AnZwDev.ALTools.Nav2018.ALSymbols;
using AnZwDev.ALTools.Nav2018.Extensions;
using AnZwDev.ALTools.Nav2018.TypeInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Nav2018.CodeTransformations
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

        protected string GetFieldCaption(PageFieldSyntax node, out bool hasCaptionProperty)
        {
            //try to find source field caption
            string caption = null;
            hasCaptionProperty = false;
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
                        {
                            caption = tableField.Caption;
                            hasCaptionProperty = true;
                        }
                    }
                    if (String.IsNullOrWhiteSpace(caption))
                        caption = source.Replace("\"", "");
                }
            }
            return caption;
        }

    }
}
