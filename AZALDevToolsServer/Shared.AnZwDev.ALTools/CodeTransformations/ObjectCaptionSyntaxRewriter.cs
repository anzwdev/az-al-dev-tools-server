using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class ObjectCaptionSyntaxRewriter : ALCaptionsSyntaxRewriter
    {

        public ObjectCaptionSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitTable(TableSyntax node)
        {
            PropertySyntax propertySyntax = node.GetProperty("Caption");
            if (propertySyntax == null)
            {
                NoOfChanges++;
                node = node.AddPropertyListProperties(
                    this.CreateCaptionPropertyFromName(node));
            }
            else
                node = UpdateCaptionFromName(node, propertySyntax);
            return base.VisitTable(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            PropertySyntax propertySyntax = node.GetProperty("Caption");
            if (propertySyntax == null)
            {
                NoOfChanges++;
                node = node.AddPropertyListProperties(
                    this.CreateCaptionPropertyFromName(node));
            }
            else
                node = UpdateCaptionFromName(node, propertySyntax);
            return base.VisitPage(node);
        }

        public override SyntaxNode VisitReport(ReportSyntax node)
        {
            PropertySyntax propertySyntax = node.GetProperty("Caption");
            if (propertySyntax == null)
            {
                NoOfChanges++;
                node = node.AddPropertyListProperties(
                    this.CreateCaptionPropertyFromName(node));
            }
            else
                node = UpdateCaptionFromName(node, propertySyntax);
            return base.VisitReport(node);
        }

        public override SyntaxNode VisitQuery(QuerySyntax node)
        {
            PropertySyntax propertySyntax = node.GetProperty("Caption");
            if (propertySyntax == null)
            {
                NoOfChanges++;
                node = node.AddPropertyListProperties(
                    this.CreateCaptionPropertyFromName(node));
            }
            else
                node = UpdateCaptionFromName(node, propertySyntax);
            return base.VisitQuery(node);
        }

        public override SyntaxNode VisitXmlPort(XmlPortSyntax node)
        {
            PropertySyntax propertySyntax = node.GetProperty("Caption");
            if (propertySyntax == null)
            {
                NoOfChanges++;
                node = node.AddPropertyListProperties(
                    this.CreateCaptionPropertyFromName(node));
            }
            else
                node = UpdateCaptionFromName(node, propertySyntax);
            return base.VisitXmlPort(node);
        }

    }
}
