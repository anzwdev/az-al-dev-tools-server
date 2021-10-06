using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddFieldCaptionsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<FieldCaptionSyntaxRewriter>
    {

        public static string LockRemovedFieldsParameterName = "lockRemovedFields";

        public AddFieldCaptionsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addFieldCaptions")
        {
        }

        protected override void SetParameters(string sourceCode, string path, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, path, span, parameters);
            this.SyntaxRewriter.LockRemovedFields = parameters.GetBoolValue(LockRemovedFieldsParameterName);
        }


    }
}
