using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AnZwDev.ALTools.ALSymbols.ALAppPackages;
using System.IO.Compression;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{
    
    public static class AppPackageSymbolReferencesReader
    {

        public static ALAppSymbolReference Deserialize(string path)
        {
            ALAppSymbolReference symbolReference = null;

            Stream packageStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            packageStream.Seek(AppPackageDataStream.HeaderLength, SeekOrigin.Begin);

            AppPackageDataStream dataStream = new AppPackageDataStream(packageStream);

            ZipArchive package = new ZipArchive(dataStream, ZipArchiveMode.Read);
            ZipArchiveEntry symbols = package.GetEntry("SymbolReference.json");
            using (Stream symbolsStream = symbols.Open())
            using (StreamReader streamReader = new StreamReader(symbolsStream))
            using (JsonReader reader = new JsonTextReader(streamReader))
            {
                JsonSerializer serializer = new JsonSerializer();
                symbolReference = serializer.Deserialize<ALAppSymbolReference>(reader);
            }
            package.Dispose();

            packageStream.Close();
            packageStream.Dispose();
            dataStream.Dispose();

            return symbolReference;
        }

    }

}
