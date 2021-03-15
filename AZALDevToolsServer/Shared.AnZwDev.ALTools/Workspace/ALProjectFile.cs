using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectFile
    {

        #region Public properties

        public ALProject Project { get; set; }
        public string RelativePath { get; private set; }

        public string FullPath
        {
            get { return (this.Project != null) ? Path.Combine(this.Project.RootPath, this.RelativePath) : this.RelativePath; }
        }

        private List<ALAppObject> _symbols;
        public List<ALAppObject> Symbols
        {
            get { return _symbols; }
            set
            {
                if (_symbols != value)
                {
                    if (_symbols != null)
                        this.Project.Symbols.RemoveObjects(_symbols);
                    _symbols = value;
                    if (_symbols != null)
                        this.Project.Symbols.AddObjects(_symbols);
                }
            }
        }

        private bool _isDirty;
        public bool IsDirty 
        { 
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                if (!_isDirty)
                    _syntaxTree = null;
            }
        }

        #endregion

        protected SyntaxTree _syntaxTree;

        #region Initialization

        public ALProjectFile() : this(null, null)
        {
        }

        public ALProjectFile(ALProject project, string relativePath)
        {
            this.Project = project;
            this.RelativePath = relativePath;
            this.IsDirty = false;
            _syntaxTree = null;
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

        #region Symbols compilation

        public void CompileSymbolReferences()
        {
            if ((this.IsDirty) && (_syntaxTree != null))
            {
                this.Symbols = this.Project.Workspace.SymbolReferenceCompiler.CreateObjectsList(_syntaxTree);
                this.IsDirty = false;
            }
            else
                this.CompileSymbolReferences(this.ReadAllText());
        }

        public void CompileSymbolReferences(string source)
        {
            this.Symbols = this.Project.Workspace.SymbolReferenceCompiler.CreateObjectsList(source);
            this.IsDirty = false;
        }

        #endregion

        public void OnAdd()
        {
            this.IsDirty = false;
            this.CompileSymbolReferences();
        }

        public void OnOpen()
        {
        }

        public void OnClose()
        {
            this.IsDirty = false;
            this.CompileSymbolReferences();
        }

        public void OnChange(string content)
        {
            this.IsDirty = true;
            if (content == null)
                content = this.ReadAllText();
            _syntaxTree = SyntaxTree.ParseObjectText(content);
        }

        public void OnSave()
        {
            this.IsDirty = false;
            this.CompileSymbolReferences();
        }

        public void OnDelete()
        {
            this.IsDirty = false;
            this.Symbols = null;
        }

        public void OnRename(string newRelativePath)
        {
            this.RelativePath = newRelativePath;
        }

    }
}
