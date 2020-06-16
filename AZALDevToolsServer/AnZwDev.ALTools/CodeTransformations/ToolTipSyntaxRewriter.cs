using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
  public class ToolTipSyntaxRewriter : ALSyntaxRewriter
  {


    public ToolTipSyntaxRewriter()
    {
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

    /* public override SyntaxNode VisitPageUserControl(PageUserControlSyntax node)
     {
         if (this.HasToolTip(node))
             return base.VisitPageUserControl(node);
         this.NoOfChanges++;
         return node.AddPropertyListProperties(this.CreateToolTipProperty(node));
     }*/

    /*public override SyntaxNode VisitPagePart(PagePartSyntax node)
    {
        if (this.HasToolTip(node))
            return base.VisitPagePart(node);
        this.NoOfChanges++;
        return node.AddPropertyListProperties(this.CreateToolTipProperty(node));
    }

    public override SyntaxNode VisitPageSystemPart(PageSystemPartSyntax node)
    {
        if (this.HasToolTip(node))
            return base.VisitPageSystemPart(node);
        this.NoOfChanges++;
        return node.AddPropertyListProperties(this.CreateToolTipProperty(node));
    }

    public override SyntaxNode VisitPageChartPart(PageChartPartSyntax node)
    {
        if (this.HasToolTip(node))
            return base.VisitPageChartPart(node);
        this.NoOfChanges++;
        return node.AddPropertyListProperties(this.CreateToolTipProperty(node));
    }*/

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
        ToolTipValue = "Executes the action " + ObjName;
      else if (node.Kind == SyntaxKind.PageField)
        ToolTipValue = "Specifies the value for field " + ObjName;

      return SyntaxFactory.PropertyLiteral(PropertyKind.ToolTip, ToolTipValue)
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
    }

  }
}
