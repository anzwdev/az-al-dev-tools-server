﻿using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Logging;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.CommandLine;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{

#if BC
    public class SemanticModelWorkspaceCommand: WorkspaceCommand
    {

        public SemanticModelWorkspaceCommand(ALDevToolsServer alDevToolsServer, string name) : base(alDevToolsServer, name)
        {
        }

        public override WorkspaceCommandResult Run(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters)
        {
            SyntaxTree sourceSyntaxTree = null;
            string newSourceCode = null;
            bool success = true;
            string errorMessage = null;

            //load project and single file
            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            Compilation compilation = this.LoadProject(projectPath, syntaxTrees, sourceCode, filePath, out sourceSyntaxTree);
            ALProject project = this.ALDevToolsServer.Workspace.FindProject(projectPath, true);

            if (!String.IsNullOrWhiteSpace(filePath))
            {
                if (!String.IsNullOrEmpty(sourceCode))
                {
                    (newSourceCode, success, errorMessage) = this.ProcessSourceCode(sourceSyntaxTree, compilation, project, range, parameters);
                    if (!success)
                        return new WorkspaceCommandResult(newSourceCode, true, errorMessage);
                }
            }
            else
                (success, errorMessage) = this.ProcessDirectory(syntaxTrees, compilation, project, parameters);

            if (success)
                return new WorkspaceCommandResult(newSourceCode);
            return new WorkspaceCommandResult(newSourceCode, true, errorMessage);
        }

        #region Project loading

        protected Compilation LoadProject(string projectPath, List<SyntaxTree> syntaxTrees, string sourceCode, string sourcePath, out SyntaxTree sourceSyntaxTree)
        {
            //load all syntax trees
            syntaxTrees.Clear();
            this.LoadProjectALFiles(projectPath, syntaxTrees, sourceCode, sourcePath, out sourceSyntaxTree);

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

        protected void LoadProjectALFiles(string projectPath, List<SyntaxTree> syntaxTrees, string sourceCode, string sourcePath, out SyntaxTree sourceSyntaxTree)
        {
            bool useSource = (!String.IsNullOrWhiteSpace(sourcePath));
            if (useSource)
                sourcePath = Path.GetFullPath(sourcePath);
            sourceSyntaxTree = null;

            string[] filePaths = Directory.GetFiles(projectPath, "*.al", SearchOption.AllDirectories);
            for (int i = 0; i < filePaths.Length; i++)
            {
                bool sourceFile = ((useSource) && (sourcePath.Equals(Path.GetFullPath(filePaths[i]))));
                string content;
                if (sourceFile)
                    content = sourceCode;
                else
                    content = File.ReadAllText(filePaths[i]);
                SyntaxTree syntaxTree = SyntaxTree.ParseObjectText(content, filePaths[i]);
                syntaxTrees.Add(syntaxTree);

                if (sourceFile)
                    sourceSyntaxTree = syntaxTree;
            }
        }


        #endregion

        #region Project files processing

        protected (bool, string) ProcessDirectory(List<SyntaxTree> syntaxTrees, Compilation compilation, ALProject project, Dictionary<string, string> parameters)
        {
            foreach (SyntaxTree syntaxTree in syntaxTrees)
            {
                (bool success, string errorMessage) = this.ProcessFile(syntaxTree, compilation, project, null, parameters);
                if (!success)
                    return (false, errorMessage);
            }
            return (true, null);
        }

        protected (bool, string) ProcessFile(SyntaxTree syntaxTree, Compilation compilation, ALProject project, Range range, Dictionary<string, string> parameters)
        {
            SyntaxNode rootNode = syntaxTree.GetRoot();
            if (rootNode != null)
            {
                (string newSource, bool success, string errorMessage) = this.ProcessSourceCode(syntaxTree, compilation, project, range, parameters);
                if ((success) && (!String.IsNullOrWhiteSpace(newSource)))
                    System.IO.File.WriteAllText(syntaxTree.FilePath, newSource);
                return (success, errorMessage);
            }
            return (true, null);
        }

        protected (string, bool, string) ProcessSourceCode(SyntaxTree syntaxTree, Compilation compilation, ALProject project, Range range, Dictionary<string, string> parameters)
        {
            try
            {
                SyntaxNode rootNode = syntaxTree.GetRoot();
                if (rootNode != null)
                {
                    //convert range to TextSpan
                    TextSpan span = new TextSpan(0, 0);

                    //fix nodes
                    SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);
                    SyntaxNode newRootNode = this.ProcessSyntaxNode(syntaxTree, rootNode, semanticModel, project, span, parameters);

                    //return new source code
                    if (newRootNode != null)
                        return (newRootNode.ToFullString(), true, null);
                }
                return (null, true, null);
            }
            catch (Exception ex)
            {
                string filePath = syntaxTree.FilePath;
                string errorMessage = (String.IsNullOrEmpty(filePath)) ?
                    $"Workspace command {this.Name} error during processing source code" : $"Workspace command {this.Name} error during processing file '{filePath}'";
                MessageLog.LogError(ex, errorMessage + ": ");
                return (null, false, errorMessage);
            }
        }

        public virtual SyntaxNode ProcessSyntaxNode(SyntaxTree syntaxTree, SyntaxNode node, SemanticModel semanticModel, ALProject project, TextSpan span, Dictionary<string, string> parameters)
        {
            bool skipFormatting = ((parameters != null) && (parameters.ContainsKey("skipFormatting")) && (parameters["skipFormatting"] == "true"));
            if (!skipFormatting)
                node = FormatSyntaxNode(node);
            return node;
        }

        #endregion

    }
#endif
}
