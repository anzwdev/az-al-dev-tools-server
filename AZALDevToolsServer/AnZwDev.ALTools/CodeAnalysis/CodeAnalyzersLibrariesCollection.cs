using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.CodeAnalysis
{
    public class CodeAnalyzersLibrariesCollection
    {

        public ALDevToolsServer ALDevToolsServer { get; }
        protected Dictionary<string, CodeAnalyzersLibrary> LibrariesCache { get; }

        public CodeAnalyzersLibrariesCollection(ALDevToolsServer newALDevToolsServer)
        {
            this.ALDevToolsServer = newALDevToolsServer;
            this.LibrariesCache = new Dictionary<string, CodeAnalyzersLibrary>();
        }

        public CodeAnalyzersLibrary GetCodeAnalyzersLibrary(string name)
        {
            if (this.LibrariesCache.ContainsKey(name))
                return this.LibrariesCache[name];

            try
            {
                CodeAnalyzersLibrary library = new CodeAnalyzersLibrary(this.ALDevToolsServer, name);
                library.Load();
                this.LibrariesCache.Add(name, library);
                return library;
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}
