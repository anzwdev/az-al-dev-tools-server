using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
#if BC
using Microsoft.Dynamics.Nav.CodeAnalysis.Workspaces.Formatting;
#endif
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class WorkspaceCommand
    {

        public static string NoOfChangesParameterName = "noOfChanges";
        public static string NoOfChangedFilesParameterName = "noOfChangedFiles";

        public ALDevToolsServer ALDevToolsServer { get; }
        public string Name { get; set; }

        public WorkspaceCommand(ALDevToolsServer alDevToolsServer, string newName)
        {
            this.ALDevToolsServer = alDevToolsServer;
            this.Name = newName;
        }

        public virtual WorkspaceCommandResult Run(string sourceCode, string path, Range range, Dictionary<string, string> parameters)
        {
            return WorkspaceCommandResult.Empty;
        }

        protected SyntaxNode FormatSyntaxNode(SyntaxNode node)
        {
#if BC
            if (node == null)
                return node;
            return Formatter.Format(node, this.GetWorkspace());
#else
            return node;
#endif
        }


#if BC
        private Microsoft.Dynamics.Nav.EditorServices.Protocol.VsCodeWorkspace _workspace = null;
        protected Microsoft.Dynamics.Nav.EditorServices.Protocol.VsCodeWorkspace GetWorkspace()
        {
            if (this._workspace == null)
                this._workspace =
                    new Microsoft.Dynamics.Nav.EditorServices.Protocol.VsCodeWorkspace();
            return this._workspace;
        }
#endif

    }
}
