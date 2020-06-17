using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
  public class ToolTipSyntaxRewriter : ALSyntaxRewriter
  {
    public string PageFieldTooltip { get; set; }
    public string PageActionTooltip { get; set; }
    public ToolTipSyntaxRewriter()
    {
      PageActionTooltip = "Executes the action %1";
      PageFieldTooltip = "Specifies the value for the field %1";
    }

    protected override SyntaxNode AfterVisitSourceCode(SyntaxNode node)
    {
      if (this.NoOfChanges == 0)
        return null;
      return base.AfterVisitSourceCode(node);
    }

    public override SyntaxNode VisitPageField(PageFieldSyntax node)
    {
      if (this.HasToolTip(node))
        return base.VisitPageField(node);
      this.NoOfChanges++;
      return node.AddPropertyListProperties(this.CreateToolTipProperty(node));
    }
    public override SyntaxNode VisitPageAction(PageActionSyntax node)
    {
      if (this.HasToolTip(node))
        return base.VisitPageAction(node);
      this.NoOfChanges++;
      return node.AddPropertyListProperties(this.CreateToolTipProperty(node));
    }

    protected bool HasToolTip(SyntaxNode node)
    {
      PropertySyntax ToolTipProperty = node.GetProperty("ToolTip");
      return ((ToolTipProperty != null) && (!String.IsNullOrWhiteSpace(ToolTipProperty.Value.ToString())));
    }

    protected PropertySyntax CreateToolTipProperty(SyntaxNode node)
    {
      //calculate indent
      int indentLength = 4;
      string indent;
      SyntaxTriviaList leadingTrivia = node.GetLeadingTrivia();
      if (leadingTrivia != null)
      {
        indent = leadingTrivia.ToString();
        int newLinePos = indent.LastIndexOf("/n");
        if (newLinePos >= 0)
          indent = indent.Substring(newLinePos + 1);
        indentLength += indent.Length;
      }
      indent = "".PadLeft(indentLength);

      SyntaxTriviaList leadingTriviaList = SyntaxFactory.ParseLeadingTrivia(indent, 0);
      SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);

      string ToolTipValue = "";
      string ObjName = node.GetProperty("Caption") != null ? node.GetProperty("Caption").Value.ToString() : node.GetNameStringValue();
      ObjName = ObjName.Replace("'", "");
      if (node.Kind == SyntaxKind.PageAction)
      {
        ToolTipValue = PageActionTooltip;
        if (ToolTipValue.Contains("%1"))
          ToolTipValue = ToolTipValue.Replace("%1", ObjName);
      }
      else if (node.Kind == SyntaxKind.PageField)
      {
        ToolTipValue = PageFieldTooltip;
        if (ToolTipValue.Contains("%1"))
          ToolTipValue = ToolTipValue.Replace("%1", ObjName);
      }

      return SyntaxFactory.PropertyLiteral(PropertyKind.ToolTip, ToolTipValue)
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
    }

  }
}
