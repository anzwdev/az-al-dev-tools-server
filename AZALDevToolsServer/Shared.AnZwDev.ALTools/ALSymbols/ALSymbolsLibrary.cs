using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALSymbolsLibrary
    {

        public ALSymbolInformation Root { get; set; }

        public ALSymbolsLibrary() : this(null)
        {
        }

        public ALSymbolsLibrary(ALSymbolInformation rootSymbol)
        {
            this.Root = rootSymbol;
        }

        public virtual bool Load(bool forceReload)
        {
            return false;
        }

        public ALSymbolInformation GetObjectsTree()
        {
            return this.Root.GetObjectsTree();
        }

        public List<ALSymbolInformation> GetSymbolsListByPath(int[][] paths, ALSymbolKind kind)
        {
            List<ALSymbolInformation> symbolsList = new List<ALSymbolInformation>();
            for (int i=0; i<paths.Length;i++)
            {
                ALSymbolInformation symbol = this.GetSymbolByPath(paths[i]);
                if ((symbol != null) && (
                    (kind == ALSymbolKind.Undefined) ||
                    (symbol.kind == kind) ||
                    ((kind == ALSymbolKind.AnyALObject) && (symbol.kind.IsObjectDefinition()))))
                    symbolsList.Add(symbol);
            }
            return symbolsList;
        }

        public ALSymbolInformation GetSymbolByPath(int[] path)
        {
            if ((this.Root != null) && (path != null) && (path.Length > 0))
            {
                ALSymbolInformation symbol = this.Root;
                for (int i = path.Length - 1; i >= 0; i--)
                {
                    if ((symbol.childSymbols == null) || (path[i] >= symbol.childSymbols.Count))
                        return null;
                    if (path[i] == -1)
                        symbol = this.Root;
                    else
                        symbol = symbol.childSymbols[path[i]];
                }
                return symbol;
            }
            return null;
        }

    }
}
