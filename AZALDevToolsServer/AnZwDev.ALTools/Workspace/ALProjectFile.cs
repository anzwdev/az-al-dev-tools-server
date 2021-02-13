using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectFile
    {

        #region Public properties

        public ALProject Project { get; set; }

        private string _relativePath;
        public virtual string RelativePath 
        { 
            get { return _relativePath; }
            set 
            {
                if (_relativePath != value)
                {
                    _relativePath = value;
                    OnRelativePathChanged();
                }
            }
        }

        public string FullPath
        {
            get { return (this.Project != null) ? Path.Combine(this.Project.RootPath, this.RelativePath) : this.RelativePath; }
        }

        #endregion

        #region Initialization

        public ALProjectFile() : this(null, null)
        {
        }

        public ALProjectFile(ALProject project, string relativePath)
        {
            this.Project = project;
            this.RelativePath = relativePath;
        }

        #endregion

        #region File content

        public string ReadAllText()
        {
            return File.ReadAllText(this.FullPath);
        }

        public void WriteAllText(string content)
        {
            File.WriteAllText(this.FullPath, content);
        }

        #endregion

        protected virtual void OnRelativePathChanged()
        {
        }

    }
}
