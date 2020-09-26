using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class WorkspaceCommandsManager
    {

        private Dictionary<string, WorkspaceCommand> _commands;

        public WorkspaceCommandsManager()
        {
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
            SyntaxTreeWorkspaceCommandsGroup groupCommand = new SyntaxTreeWorkspaceCommandsGroup("runMultiple");

            this.RegisterCommand(new AddAppAreasWorkspaceCommand());
            this.RegisterCommand(new AddToolTipsWorkspaceCommand());
            this.RegisterCommand(new AddDataClassificationWorkspaceCommand());

            this.RegisterCommand(new RemoveWithWorkspaceCommand());

            this.RegisterCommand(groupCommand.AddCommand(new SortProceduresWorkspaceCommand()));
            this.RegisterCommand(groupCommand.AddCommand(new SortVariablesWorkspaceCommand()));
            this.RegisterCommand(groupCommand.AddCommand(new SortPropertiesWorkspaceCommand()));
            this.RegisterCommand(groupCommand.AddCommand(new SortReportColumnsWorkspaceCommand()));
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
