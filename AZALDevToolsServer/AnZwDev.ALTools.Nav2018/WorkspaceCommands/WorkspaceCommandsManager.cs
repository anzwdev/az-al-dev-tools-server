/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AnZwDev.ALTools.Nav2018.WorkspaceCommands
{
    public class WorkspaceCommandsManager
    {

        private Dictionary<string, WorkspaceCommand> _commands;

        public WorkspaceCommandsManager()
        {
            _commands = new Dictionary<string, WorkspaceCommand>();
            RegisterCommands();
        }

        protected void RegisterCommand(WorkspaceCommand command)
        {
            _commands.Add(command.Name, command);
        }

        protected virtual void RegisterCommands()
        {
            this.RegisterCommand(new AddAppAreasWorkspaceCommand());
            this.RegisterCommand(new AddToolTipsWorkspaceCommand());
        }

        public WorkspaceCommandResult RunCommand(string commandName, string sourceCode, string path, Dictionary<string, string> parameters)
        {
            if (_commands.ContainsKey(commandName))
                return _commands[commandName].Run(sourceCode, path, parameters);
            else
                throw new Exception($"Workspace command {commandName} not found.");
        }

    }
}
