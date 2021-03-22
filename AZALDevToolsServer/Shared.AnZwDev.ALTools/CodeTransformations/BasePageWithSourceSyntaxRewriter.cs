using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class BasePageWithSourceSyntaxRewriter : ALSyntaxRewriter
    {

        protected TableInformationProvider TableInformationProvider { get; }
        protected List<TableFieldInformaton> TableFields { get; set; }

        public BasePageWithSourceSyntaxRewriter()
        {
            this.TableInformationProvider = new TableInformationProvider();
            this.TableFields = null;
        }

        public override SyntaxNode VisitRequestPage(RequestPageSyntax node)
        {
            this.CheckSourceTableProperty(node);
            return base.VisitRequestPage(node);
        }

        public override SyntaxNode VisitPageExtension(PageExtensionSyntax node)
        {
            this.TableFields = null;
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
            this.TableFields = null;
            PropertyValueSyntax sourceTablePropertyValue = node.GetPropertyValue("SourceTable");
            if (sourceTablePropertyValue != null)
            {
                string sourceTable = ALSyntaxHelper.DecodeName(sourceTablePropertyValue.ToString());
                if (!String.IsNullOrWhiteSpace(sourceTable))
                    this.TableFields = this.TableInformationProvider.GetTableFields(this.Project, sourceTable, false, false);
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
                    if (this.TableFields != null)
                    {
                        TableFieldInformaton tableField = this.TableFields.Where(p => (source.Equals(p.Name, StringComparison.CurrentCultureIgnoreCase))).FirstOrDefault();
                        if ((tableField != null) && (!String.IsNullOrWhiteSpace(tableField.Caption)))
                        {
                            caption = tableField.Caption;
                            hasCaptionProperty = true;
                        }
                    }
                    if (String.IsNullOrWhiteSpace(caption))
                        caption = source.Replace("\"", "").RemovePrefixSuffix(this.Project.MandatoryPrefixes, this.Project.MandatorySuffixes, this.Project.MandatoryAffixes);
                }
            }
            return caption;
        }

    }
}
