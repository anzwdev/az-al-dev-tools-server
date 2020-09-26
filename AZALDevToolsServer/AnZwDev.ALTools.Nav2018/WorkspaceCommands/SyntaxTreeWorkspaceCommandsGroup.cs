﻿using AnZwDev.ALTools.Nav2018.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.WorkspaceCommands
{
    public class SyntaxTreeWorkspaceCommandsGroup : SyntaxTreeWorkspaceCommand
    {

        private Dictionary<string, SyntaxTreeWorkspaceCommand> _commands;


        public SyntaxTreeWorkspaceCommandsGroup(string name) : base(name)
        {
            this._commands = new Dictionary<string, SyntaxTreeWorkspaceCommand>();
        }

        public SyntaxTreeWorkspaceCommand AddCommand(SyntaxTreeWorkspaceCommand command)
        {
            this._commands.Add(command.Name, command);
            return command;
        }

        public override SyntaxNode ProcessSyntaxNode(SyntaxNode node, string sourceCode, string path, TextSpan span, Dictionary<string, string> parameters)
        {
            string commandsParameterValue = parameters["commandsList"];
            char[] sep = { ',' };
            string[] commandsList = commandsParameterValue.Split(sep);
            for (int i=0;i<commandsList.Length; i++)
            {
                if (this._commands.ContainsKey(commandsList[i]))
                    node = this._commands[commandsList[i]].ProcessSyntaxNode(node, sourceCode, path, span, parameters);
            }
            return base.ProcessSyntaxNode(node, sourceCode, path, span, parameters);
        }


    }
}
