using AnZwDev.ALTools.ALProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.CodeAnalysis
{
    //default analyzers
    //"${AppSourceCop}","${CodeCop}","${PerTenantExtensionCop}","${UICop}"

    public class CodeAnalyzersLibrary
    {

        public ALDevToolsServer ALDevToolsServer { get; }
        public string Name { get; }
        public string FilePath { get; }
        public List<CodeAnalyzerRule> Rules { get; }

        public CodeAnalyzersLibrary(ALDevToolsServer newALDevToolsServer, string newName)
        {
            this.ALDevToolsServer = newALDevToolsServer;

            newName = newName.Trim();
            if ((newName.StartsWith("${")) && (newName.EndsWith("}")))
            {
                newName = newName.Substring(2, newName.Length - 3);
                this.Name = newName;
                this.FilePath = System.IO.Path.Combine(this.ALDevToolsServer.ExtensionBinPath, "Analyzers", "Microsoft.Dynamics.Nav." + this.Name + ".dll");
            }
            else
            {
                this.FilePath = newName;
                this.Name = System.IO.Path.GetFileNameWithoutExtension(this.FilePath);
            }

            this.Rules = new List<CodeAnalyzerRule>();
        }

        public void Load()
        {
            this.Rules.Clear();

            Assembly codeAnalyzerAssembly = Assembly.LoadFrom(this.FilePath);

            foreach (Module module in codeAnalyzerAssembly.Modules)
            {
                try
                {
                    Type[] types = module.GetTypes();
                    foreach (Type type in types)
                    {
                        if (IsAnalyzer(type))
                        {
                            dynamic analyzer = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                            dynamic diagnostics = analyzer.SupportedDiagnostics;
                            if (diagnostics != null)
                            {
                                foreach (dynamic diag in diagnostics)
                                {
                                    CodeAnalyzerRule rule = new CodeAnalyzerRule();
                                    rule.id = diag.Id.ToString();
                                    rule.title = diag.Title.ToString();
                                    rule.description = diag.Description.ToString();
                                    rule.category = diag.Category.ToString();
                                    rule.defaultSeverity = diag.DefaultSeverity.ToString();
                                    rule.isEnabledByDefault = diag.IsEnabledByDefault;
                                    this.Rules.Add(rule);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex is System.Reflection.ReflectionTypeLoadException)
                    {
                        var typeLoadException = ex as ReflectionTypeLoadException;
                        var loaderExceptions = typeLoadException.LoaderExceptions;
                        if (loaderExceptions.Length > 0)
                            throw loaderExceptions[0];
                    }
                    throw;
                }
            }

        }

        protected bool IsAnalyzer(Type type)
        {
            IEnumerable<Attribute> attributes = type.GetCustomAttributes();
            foreach (Attribute attribute in attributes)
            {
                if (attribute.GetType().Name == "DiagnosticAnalyzerAttribute")
                    return true;
            }
            return false;
        }

    }
}
