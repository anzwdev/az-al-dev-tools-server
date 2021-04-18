using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class WorkspaceCommandsManager
    {

        public ALDevToolsServer ALDevToolsServer { get; }

        private Dictionary<string, WorkspaceCommand> _commands;

        public WorkspaceCommandsManager(ALDevToolsServer alDevToolsServer)
        {
            this.ALDevToolsServer = alDevToolsServer;
            _commands = new Dictionary<string, WorkspaceCommand>();
            RegisterCommands();
        }

        protected WorkspaceCommand RegisterCommand(WorkspaceCommand command)
        {
            _commands.Add(command.Name, command);
            return command;
        }

        protected virtual void RegisterCommands()
        {
            SyntaxTreeWorkspaceCommandsGroup groupCommand = new SyntaxTreeWorkspaceCommandsGroup(this.ALDevToolsServer, "runMultiple");

            this.RegisterCommand(new AddAppAreasWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddToolTipsWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddPageControlCaptionWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddDataClassificationWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddFieldCaptionsWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddObjectCaptionsWorkspaceCommand(this.ALDevToolsServer));

#if BC            
            this.RegisterCommand(new RemoveWithWorkspaceCommand(this.ALDevToolsServer));
#endif
            this.RegisterCommand(new RemoveVariableWorkspaceCommand(this.ALDevToolsServer));

            this.RegisterCommand(groupCommand.AddCommand(new SortProceduresWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand.AddCommand(new SortVariablesWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand.AddCommand(new SortPropertiesWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand.AddCommand(new SortReportColumnsWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand.AddCommand(new SortTableFieldsWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand);
        }

        public WorkspaceCommandResult RunCommand(string commandName, string sourceCode, string path, Range range, Dictionary<string, string> parameters)
        {
            if (_commands.ContainsKey(commandName))
                return _commands[commandName].Run(sourceCode, path, range, parameters);
            else
                throw new Exception($"Workspace command {commandName} not found.");
        }

    }
}
