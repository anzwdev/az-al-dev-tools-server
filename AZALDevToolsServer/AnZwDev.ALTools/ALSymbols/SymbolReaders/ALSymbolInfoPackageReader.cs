using AnZwDev.ALTools.ALProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using System.Dynamic;

namespace AnZwDev.ALTools.ALSymbols.SymbolReaders
{
    /*
    public class ALSymbolInfoPackageReader3
    {

        protected ALExtensionProxy ALExtensionProxy { get; }

        public ALSymbolInfoPackageReader(ALExtensionProxy extensionProxy)
        {
            this.ALExtensionProxy = extensionProxy;
        }

        public ALSymbolInformation ReadSymbols(string packagePath)
        {
            try
            {
                ALSymbolInformation rootSymbol = null;

                Stream packageStream = new FileStream(packagePath, FileMode.Open);
                packageStream.Seek(40, SeekOrigin.Begin);
                ZipArchive package = new ZipArchive(packageStream, ZipArchiveMode.Read);
                ZipArchiveEntry symbols = package.GetEntry("SymbolReference.json");
                using (Stream symbolsStream = symbols.Open())
                using (StreamReader streamReader = new StreamReader(symbolsStream))
                using (JsonReader reader = new JsonTextReader(streamReader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    dynamic symbolsData = serializer.Deserialize(reader);
                    rootSymbol = this.ReadSymbols(symbolsData);
                }
                package.Dispose();
                packageStream.Dispose();

                return rootSymbol;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        protected ALSymbolInformation ReadSymbols(dynamic symbolsData)
        {
            ExpandoObject data = symbolsData as ExpandoObject;

            


        }


    }
    */
}
