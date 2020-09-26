﻿using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.CommandLine;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SemanticModelWorkspaceCommand: WorkspaceCommand
    {

        public SemanticModelWorkspaceCommand(string name) : base(name)
        {
        }

        public override WorkspaceCommandResult Run(string sourceCode, string path, Range range, Dictionary<string, string> parameters)
        {
            WorkspaceCommandResult outVal = null;

            //load project
            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            Compilation compilation = this.LoadProject(path, syntaxTrees);

            if (!String.IsNullOrEmpty(sourceCode))
            {
                string newSourceCode = this.ProcessSourceCode(sourceCode, compilation, range, parameters);
                outVal = new WorkspaceCommandResult(newSourceCode);
            }
            else if (!String.IsNullOrWhiteSpace(path))
            {
                int noOfModifiedFiles = this.ProcessFiles(syntaxTrees, compilation, parameters);
                outVal = new WorkspaceCommandResult(null);
                outVal.SetParameter(NoOfChangedFilesParameterName, noOfModifiedFiles.ToString());
            }
            else
                outVal = new WorkspaceCommandResult();

            return outVal;
        }

        #region Project loading

        protected Compilation LoadProject(string projectPath, List<SyntaxTree> syntaxTrees)
        {
            //load all syntax trees
            syntaxTrees.Clear();
            this.LoadProjectALFiles(projectPath, syntaxTrees);

            List<Diagnostic> diagnostics = new List<Diagnostic>();

            //load project manifest
            string projectFile = Path.Combine(projectPath, "app.json");
            ProjectManifest manifest = ProjectManifest.ReadFromString(projectFile, File.ReadAllText(projectFile), diagnostics);

            //create compilation
            Compilation compilation = Compilation.Create("MyCompilation", manifest.AppManifest.AppPublisher,
                manifest.AppManifest.AppVersion, manifest.AppManifest.AppId,
                null, syntaxTrees,
                new CompilationOptions());

            LocalCacheSymbolReferenceLoader referenceLoader =
                this.SafeCreateLocalCacheSymbolReferenceLoader(Path.Combine(projectPath, ".alpackages"));
            
            compilation = compilation
                .WithReferenceLoader(referenceLoader)
                .WithReferences(manifest.GetAllReferences());

            return compilation;
        }

        protected LocalCacheSymbolReferenceLoader SafeCreateLocalCacheSymbolReferenceLoader(string packagesCachePath)
        {
            //return new LocalCacheSymbolReferenceLoader(packagesCachePath);
            Type type = typeof(LocalCacheSymbolReferenceLoader);
            ConstructorInfo[] constructors = type.GetConstructors();
            ParameterInfo[] parameters = constructors[0].GetParameters();
            object[] parametersValues = new object[parameters.Length];
            parametersValues[0] = packagesCachePath;
            for (int i=1; i< parametersValues.Length; i++)
            {
                parametersValues[i] = null;
            }
            return (LocalCacheSymbolReferenceLoader)constructors[0].Invoke(parametersValues);
        }

        protected void LoadProjectALFiles(string projectPath, List<SyntaxTree> syntaxTrees)
        {
            string[] filePaths = Directory.GetFiles(projectPath, "*.al", SearchOption.AllDirectories);
            for (int i = 0; i < filePaths.Length; i++)
            {
                string content = File.ReadAllText(filePaths[i]);
                SyntaxTree syntaxTree = SyntaxTree.ParseObjectText(content, filePaths[i]);
                syntaxTrees.Add(syntaxTree);
            }
        }


        #endregion

        #region Project files processing

        protected int ProcessFiles(List<SyntaxTree> syntaxTrees, Compilation compilation, Dictionary<string, string> parameters)
        {
            int noOfModifiedFiles = 0;

            foreach (SyntaxTree syntaxTree in syntaxTrees)
            {
                SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);

                SyntaxNode newRootNode = this.ProcessFile(syntaxTree, semanticModel, null, parameters);
                if (newRootNode != null)
                {
                    File.WriteAllText(syntaxTree.FilePath, newRootNode.ToFullString());

                    if (newRootNode != syntaxTree.GetRoot())
                        noOfModifiedFiles++;
                }
            }

            return noOfModifiedFiles;
        }

        protected virtual SyntaxNode ProcessFile(SyntaxTree syntaxTree, SemanticModel semanticModel, Range range, Dictionary<string, string> parameters)
        {
            return syntaxTree.GetRoot();
        }

        protected string ProcessSourceCode(string sourceCode, Compilation compilation, Range range, Dictionary<string, string> parameters)
        {
            SyntaxTree syntaxTree = SyntaxTree.ParseObjectText(sourceCode);
            SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);
            SyntaxNode newRootNode = this.ProcessFile(syntaxTree, semanticModel, null, parameters);
            if (newRootNode != null)
                return newRootNode.ToFullString();
            return null;
        }

        #endregion

    }
}
