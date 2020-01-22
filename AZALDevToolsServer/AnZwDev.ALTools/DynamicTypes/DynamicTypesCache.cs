using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.DynamicTypes
{
    public class DynamicTypesCache
    {

        public string ALCompilerPath { get; set; }

        private readonly Dictionary<string, Assembly> _assemblies;
        private readonly Dictionary<string, Dictionary<string, Type>> _types;
        private readonly CSharpCodeProvider _compiler;
        private List<string> _referencedAssembliesList;

        public DynamicTypesCache()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            _assemblies = new Dictionary<string, Assembly>();
            _types = new Dictionary<string, Dictionary<string, Type>>();
            _compiler = new CSharpCodeProvider(options);
            _referencedAssembliesList = null;
            this.ALCompilerPath = "";
        }

        public Assembly GetAssemblyFromResource(string resource)
        {
            if (_assemblies.ContainsKey(resource))
                return _assemblies[resource];
            Assembly assembly = this.BuildAssemblyFromResource(resource);
            _assemblies.Add(resource, assembly);
            _types.Add(resource, new Dictionary<string, Type>());
            return assembly;
        }

        public Type GetTypeFromResource(string resource, string typeName)
        {
            if ((_types.ContainsKey(resource)) && (_types[resource].ContainsKey(typeName)))
                return _types[resource][typeName];
            Assembly assembly = this.GetAssemblyFromResource(resource);
            Type type = assembly.GetType(typeName);
            _types[resource].Add(typeName, type);
            return type;
        }

        protected Assembly BuildAssemblyFromResource(string resourceName)
        {
            var assembly = this.GetType().Assembly; // Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return BuildAssembly(result);
            }
        }

        public Assembly BuildAssembly(string sourceCode)
        {
            var parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;

            //add referenced assemblies
            if (_referencedAssembliesList == null)
                GetReferencedAssemblies();
            foreach (string assemblyPath in _referencedAssembliesList)
            {
                parameters.ReferencedAssemblies.Add(assemblyPath);
            }

            var result = _compiler.CompileAssemblyFromSource(parameters, sourceCode);

            if (result.Errors.Count > 0)
            {
                throw new Exception("Assembly could not be created.");
            }

            return result.CompiledAssembly;
        }

        private void GetReferencedAssemblies()
        {
            _referencedAssembliesList = new List<string>();
            this.GetAppAssemblies();
            this.GetALCompilerAssemblies();
        }

        private void GetAppAssemblies()
        {
            Assembly[] assemblyList = AppDomain.CurrentDomain.GetAssemblies();

            HashSet<string> assemblyNameList = new HashSet<string>();
            assemblyNameList.Add("mscorlib.dll");
            assemblyNameList.Add("Microsoft.CSharp.dll");
            assemblyNameList.Add("System.dll");
            assemblyNameList.Add("System.Core.dll");
            assemblyNameList.Add("System.Data.dll");
            assemblyNameList.Add("System.Net.Http.dll");
            assemblyNameList.Add("System.Xml.dll");

            foreach (Assembly assembly in assemblyList)
            {
                if (!String.IsNullOrWhiteSpace(assembly.Location))
                {
                    string fileName = Path.GetFileName(assembly.Location);
                    if (assemblyNameList.Contains(fileName))
                        _referencedAssembliesList.Add(assembly.Location);
                    else
                        Console.WriteLine(assembly.Location);
                }
            }
        }

        private void GetALCompilerAssemblies()
        {
            if (!String.IsNullOrWhiteSpace(this.ALCompilerPath))
            {
                _referencedAssembliesList.Add(Path.Combine(this.ALCompilerPath, "Microsoft.Dynamics.Nav.CodeAnalysis.dll"));
                //_referencedAssembliesList.Add(Path.Combine(this.ALCompilerPath, "System.Collections.Immutable.dll"));
            }
        }

    }
}
