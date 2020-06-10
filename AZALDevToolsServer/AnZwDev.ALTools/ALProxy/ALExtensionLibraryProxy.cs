using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace AnZwDev.ALTools.ALProxy
{
    public class ALExtensionLibraryProxy
    {

        public string Path { get; }
        public Assembly LibraryAssembly { get; }
        
        public ALExtensionLibraryProxy(string path)
        {
            this.Path = path;
            if (System.IO.File.Exists(this.Path))
                this.LibraryAssembly = Assembly.LoadFrom(path);
        }

    }
}
