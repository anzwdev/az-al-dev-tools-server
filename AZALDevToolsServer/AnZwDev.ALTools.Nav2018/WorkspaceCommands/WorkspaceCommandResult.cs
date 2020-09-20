using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Nav2018.WorkspaceCommands
{
    public class WorkspaceCommandResult
    {

        public string Source { get; set; }
        public Dictionary<string, string> Parameters { get; set; }

        private static WorkspaceCommandResult _empty = null;
        public static WorkspaceCommandResult Empty 
        { 
            get 
            {
                if (_empty == null)
                    _empty = new WorkspaceCommandResult();
                return _empty;
            } 
        }

        public WorkspaceCommandResult()
        {
            this.Source = null;
            this.Parameters = null;
        }

        public WorkspaceCommandResult(string newSource)
        {
            this.Source = newSource;
            this.Parameters = null;
        }

        public void SetParameter(string name, string value)
        {
            if (this.Parameters == null)
                this.Parameters = new Dictionary<string, string>();
            if (this.Parameters.ContainsKey(name))
                this.Parameters[name] = value;
            else           
                this.Parameters.Add(name, value);
        }

    }
}
