using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectSymbolsLibrarySource : ALAppBaseSymbolsLibrarySource
    {

        public ALProject Project { get; }

        public ALProjectSymbolsLibrarySource(ALProject project)
        {
            this.Project = project;
        }

        public override ALSymbolSourceLocation GetSymbolSourceLocation(ALSymbol symbol)
        {
            ALSymbolSourceLocation location = new ALSymbolSourceLocation(symbol);

            ALAppObject alAppObject = null;
            if (this.Project.Symbols != null)
            {
                alAppObject = this.Project.Symbols.FindObjectByName(symbol.kind, symbol.name);
                if (alAppObject != null)
                {
                    this.SetSource(location, this.Project.Symbols, alAppObject);
                    return location;
                }
            }

            if (this.Project.Dependencies != null)
            {
                foreach (ALProjectDependency dependency in this.Project.Dependencies)
                {
                    if (dependency.Symbols != null)
                    {
                        alAppObject = dependency.Symbols.FindObjectByName(symbol.kind, symbol.name);
                        if (alAppObject != null)
                        {
                            this.SetSource(location, dependency.Symbols, alAppObject);
                            return location;
                        }
                    }
                }
            }

            return location;
        }



    }
}
