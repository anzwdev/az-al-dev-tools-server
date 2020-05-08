using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace AnZwDev.ALTools.ALProxy
{
    public class ALExtensionProxy
    {

        public ALExtensionLibraryProxy CodeAnalysis { get; private set; }
        public ALExtensionLibraryProxy System_Collections_Immutable { get; private set; }
        public ALExtensionLibraryProxy CompilerServicesUnsafe { get; private set; }

        public ALExtensionProxy()
        {
            this.BCVersion = BCVersion.Undefined;
        }

        public BCVersion BCVersion { get; private set; }

        public void Load(string libraryPath)
        {
            this.CompilerServicesUnsafe = new ALExtensionLibraryProxy(Path.Combine(libraryPath, "System.Runtime.CompilerServices.Unsafe.dll"));

            this.System_Collections_Immutable = new ALExtensionLibraryProxy(Path.Combine(libraryPath, "System.Collections.Immutable.dll"));
            this.CodeAnalysis = new ALExtensionLibraryProxy(Path.Combine(libraryPath, "Microsoft.Dynamics.Nav.CodeAnalysis.dll"));
            this.DetectRuntime();
        }

        protected void DetectRuntime()
        {
            if (this.CodeAnalysis.LibraryAssembly != null)
            {
                Version version = this.CodeAnalysis.LibraryAssembly.GetName().Version;
                switch (version.Major)
                {
                    case 0:
                        this.BCVersion = BCVersion.NAV2018;
                        break;
                    case 1:
                        this.BCVersion = BCVersion.BCv1_0;
                        break;
                    case 2:
                        this.BCVersion = BCVersion.BCv2_0;
                        break;
                    case 3:
                        this.BCVersion = BCVersion.BCv3_0;
                        break;
                    case 4:
                        this.BCVersion = BCVersion.BCv4_0;
                        break;
                    default:
                        if (version.Major > 4)
                            this.BCVersion = BCVersion.BC_undefined;
                        break;
                }
            }
        }

        public dynamic GetSyntaxTree(string source)
        {
            ALExtensionLibraryTypeProxy syntaxTree = new ALExtensionLibraryTypeProxy(
                this.CodeAnalysis,
                "Microsoft.Dynamics.Nav.CodeAnalysis.Syntax.SyntaxTree");

            dynamic sourceTree = null;

            if (this.BCVersion == BCVersion.NAV2018)
            {
                try
                {
                    //ParseObjectText(string text, string path = null, Encoding encoding = null, CancellationToken cancellationToken = default(CancellationToken));
                    sourceTree = syntaxTree.CallStaticMethod("ParseObjectText", source,
                        Type.Missing, Type.Missing, Type.Missing);
                }
                catch (Exception)
                {
                    sourceTree = null;
                }
            }

            if (sourceTree == null)
            {
                //ParseObjectText(string text, string path = null, Encoding encoding = null, ParseOptions options = null, CancellationToken cancellationToken = default(CancellationToken));
                sourceTree = syntaxTree.CallStaticMethod("ParseObjectText", source,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }

            return sourceTree;
        }

    }
}
