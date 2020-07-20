/****************************************************************
 *                                                              *
 * Legacy version of the library maintained to support Nav 2018 *
 *                                                              *
 ****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Nav2018.ALSymbols
{
    public class ALSymbolLibrariesCollection
    {

        private int _libraryId;
        private Dictionary<int, ALSymbolsLibrary> _libraries;

        public ALSymbolLibrariesCollection()
        {
            this._libraryId = 0;
            this._libraries = new Dictionary<int, ALSymbolsLibrary>();
        }

        public int AddLibrary(ALSymbolsLibrary library)
        {
            this._libraryId++;
            this._libraries.Add(this._libraryId, library);
            return this._libraryId;
        }

        public void RemoveLibrary(int id)
        {
            if (this._libraries.ContainsKey(id))
                this._libraries.Remove(id);
        }

        public ALSymbolsLibrary GetLibrary(int id)
        {
            if (this._libraries.ContainsKey(id))
                return this._libraries[id];
            return null;
        }

    }
}
