using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using System.Dynamic;
using AnZwDev.ALTools.ALSymbols.ALAppPackages;

namespace AnZwDev.ALTools.ALSymbols.SymbolReaders
{

    public class ALSymbolInfoPackageReader
    {

        public ALSymbolInfoPackageReader()
        {
        }

        public ALAppPackage ReadAppPackage(string packagePath)
        {
            try
            {
                ALAppPackage appPackage = null;

                Stream packageStream = new FileStream(packagePath, FileMode.Open);
                packageStream.Seek(40, SeekOrigin.Begin);

                MemoryStream memoryStream = new MemoryStream();
                packageStream.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                packageStream.Close();
                packageStream.Dispose();

                ZipArchive package = new ZipArchive(memoryStream, ZipArchiveMode.Read);
                ZipArchiveEntry symbols = package.GetEntry("SymbolReference.json");
                using (Stream symbolsStream = symbols.Open())
                using (StreamReader streamReader = new StreamReader(symbolsStream))
                using (JsonReader reader = new JsonTextReader(streamReader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    appPackage = serializer.Deserialize<ALAppPackage>(reader);
                }
                package.Dispose();
                memoryStream.Dispose();

                return appPackage;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ALSymbolInformation ReadSymbols(string packagePath)
        {
            ALAppPackage alAppPackage = this.ReadAppPackage(packagePath);
            if (alAppPackage != null)
                return alAppPackage.ToALSymbol();
            return null;
        }

    }
}
