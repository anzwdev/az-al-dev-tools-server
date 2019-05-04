using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALProjectFile
    {

        public string id { get; set; }
        public string name { get; set; }
        public string publisher { get; set; }
        public string version { get; set; }
        public string platform { get; set; }
        public string application { get; set; }
        public string test { get; set; }
        public ALProjectFileDependency[] dependencies { get; set; }

        public ALProjectFile()
        {
        }

        public static ALProjectFile Load(string path)
        {
            string jsonText = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<ALProjectFile>(jsonText);
        }

    }
}
