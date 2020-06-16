using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddToolTipsWorkspaceCommand: SyntaxRewriterWorkspaceCommand<ToolTipSyntaxRewriter>
    {
        public AddToolTipsWorkspaceCommand(): base("addToolTips")
        {
        }

        protected override ToolTipSyntaxRewriter CreateSyntaxRewriter(string sourceCode, string path, Dictionary<string, string> parameters)
        {
            ToolTipSyntaxRewriter syntaxRewriter = base.CreateSyntaxRewriter(sourceCode, path, parameters);
            return syntaxRewriter;
        }

    }
}
