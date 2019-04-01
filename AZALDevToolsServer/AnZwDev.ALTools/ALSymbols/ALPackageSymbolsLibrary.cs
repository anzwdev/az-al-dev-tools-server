using AnZwDev.ALTools.ALProxy;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALPackageSymbolsLibrary : ALSymbolsLibrary
    {
        public ALExtensionProxy ALExtensionProxy { get; }

        public string Path { get; private set; }

        public string PublisherName { get; set; }
        public string PackageName { get; set; }
        public VersionNumber Version { get; set; }

        protected DateTime _packageFileCreationTime;
        protected DateTime _packageFileLastWriteTime;
        protected long _packageFileSize;

        public ALPackageSymbolsLibrary(ALExtensionProxy alExtensionProxy, string filePath)
        {
            this.ALExtensionProxy = alExtensionProxy;
            this.Path = filePath;

            this._packageFileCreationTime = DateTime.Now;
            this._packageFileLastWriteTime = DateTime.Now;
            this._packageFileSize = -1;
        }

        public override bool Load(bool forceReload)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(this.Path);
                if ((!forceReload) &&
                    (fileInfo.LastWriteTime == this._packageFileLastWriteTime) &&
                    (fileInfo.Length == this._packageFileSize) &&
                    (fileInfo.CreationTime == this._packageFileCreationTime))
                    return true;

                this._packageFileLastWriteTime = fileInfo.LastWriteTime;
                this._packageFileSize = fileInfo.Length;
                this._packageFileCreationTime = fileInfo.CreationTime;

                ALSymbolInfoPackageReader reader = new ALSymbolInfoPackageReader(this.ALExtensionProxy);
                this.Root = reader.ReadSymbols(this.Path);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
