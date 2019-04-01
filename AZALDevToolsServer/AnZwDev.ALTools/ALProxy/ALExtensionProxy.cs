using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AnZwDev.ALTools.ALProxy
{
    public class ALExtensionProxy
    {

        public ALExtensionLibraryProxy CodeAnalysis { get; private set; }
        public ALExtensionLibraryProxy System_Collections_Immutable { get; private set; }

        public ALExtensionProxy()
        {
        }

        public void Load(string libraryPath)
        {
            //this.System_Collections_Immutable = new ALExtensionLibraryProxy(Path.Combine(libraryPath, "System.Collections.Immutable.dll"));
            this.CodeAnalysis = new ALExtensionLibraryProxy(Path.Combine(libraryPath, "Microsoft.Dynamics.Nav.CodeAnalysis.dll"));
        }

    }
}
