using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
  public class AddToolTipsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<ToolTipSyntaxRewriter>
  {

    public static string FieldTooltipParameterName = "toolTipField";
    public static string ActionTooltipParameterName = "toolTipAction";
    public AddToolTipsWorkspaceCommand() : base("addToolTips")
    {
    }

    protected override ToolTipSyntaxRewriter CreateSyntaxRewriter(string sourceCode, string path, Dictionary<string, string> parameters)
    {
      ToolTipSyntaxRewriter syntaxRewriter = base.CreateSyntaxRewriter(sourceCode, path, parameters);
      if (parameters.ContainsKey(FieldTooltipParameterName))
        syntaxRewriter.PageFieldTooltip = parameters[FieldTooltipParameterName];
      if (parameters.ContainsKey(ActionTooltipParameterName))
        syntaxRewriter.PageActionTooltip = parameters[ActionTooltipParameterName];
      return syntaxRewriter;
    }

  }
}
