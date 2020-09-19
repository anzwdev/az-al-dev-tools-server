using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SortReportColumnsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortReportColumnsSyntaxRewriter>
    {

        public SortReportColumnsWorkspaceCommand(): base("sortReportColumns")
        {
        }

    }
}
