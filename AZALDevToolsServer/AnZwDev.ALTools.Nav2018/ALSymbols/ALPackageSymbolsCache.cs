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
    public class ALPackageSymbolsCache
    {
        protected Dictionary<string, ALPackageSymbolsLibrary> _cache;

        public ALPackageSymbolsCache()
        {
            _cache = new Dictionary<string, ALPackageSymbolsLibrary>();
        }

        public ALPackageSymbolsLibrary GetSymbols(string path, bool forceReload)
        {
            this.ClearInvalidPackages();
            ALPackageSymbolsLibrary symbols;
            if (!_cache.ContainsKey(path))
            {
                symbols = new ALPackageSymbolsLibrary(path);
                _cache.Add(path, symbols);
            }
            else
                symbols = _cache[path];
            symbols.Load(forceReload);
            return symbols;
        }

        public void Clear()
        {
            _cache.Clear();
        }

        public void ClearInvalidPackages()
        {
            List<string> invalidKeyValues = null;
            foreach (string keyValue in this._cache.Keys)
            {
                if (!System.IO.File.Exists(this._cache[keyValue].Path))
                {
                    if (invalidKeyValues == null)
                        invalidKeyValues = new List<string>();
                    invalidKeyValues.Add(keyValue);
                }
            }
            if (invalidKeyValues != null)
            {
                for (int i=0; i<invalidKeyValues.Count; i++)
                {
                    this._cache.Remove(invalidKeyValues[i]);
                }
            }
        }

    }
}
