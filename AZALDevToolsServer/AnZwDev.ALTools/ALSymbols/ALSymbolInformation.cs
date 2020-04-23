using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public class ALSymbolInformation
    {
        public int id { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string subtype { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string fullName { get; set; }
        
        public ALSymbolKind kind { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string source { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string extends { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ALSymbolInformation> childSymbols { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Range range { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Range selectionRange { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Range contentRange { get; set; }

        public ALSymbolInformation()
        {
            this.id = 0;
            this.childSymbols = null;
            this.fullName = null;
            this.subtype = null;
            this.source = null;
            this.extends = null;
            this.contentRange = null;
        }

        public ALSymbolInformation(ALSymbolKind kindValue, string nameValue) : this()
        {
            this.kind = kindValue;
            this.name = nameValue;
            this.fullName = ALSyntaxHelper.EncodeName(nameValue);
        }

        public ALSymbolInformation(ALSymbolKind kindValue, string nameValue, int? idValue) : this(kindValue, nameValue)
        {
            if (idValue.HasValue)
                this.id = idValue.Value;
        }

        public void AddChildSymbol(ALSymbolInformation symbolInfo)
        {
            if (symbolInfo == null)
                return;
            if (this.childSymbols == null)
                this.childSymbols = new List<ALSymbolInformation>();
            this.childSymbols.Add(symbolInfo);
        }

        public void UpdateFields()
        {
            if (String.IsNullOrWhiteSpace(this.fullName))
                this.fullName = this.kind.ToName() + " " + ALSyntaxHelper.EncodeName(this.name);

            if (this.childSymbols != null)
            {
                for (int i=0; i<this.childSymbols.Count;i++)
                {
                    this.childSymbols[i].UpdateFields();
                }
            }
        }

        public ALSymbolInformation CreateCopy(bool withChildSymbols)
        {
            ALSymbolInformation symbol = new ALSymbolInformation();

            symbol.id = this.id;
            symbol.name = this.name;
            symbol.subtype = this.subtype;
            symbol.fullName = this.fullName;
            symbol.kind = this.kind;
            symbol.range = this.range;
            symbol.selectionRange = this.selectionRange;

            if ((withChildSymbols) && (this.childSymbols != null))
            {
                for (int i=0; i<this.childSymbols.Count; i++)
                {
                    symbol.AddChildSymbol(this.childSymbols[i].CreateCopy(withChildSymbols));
                }
            }

            return symbol;
        }

        public ALSymbolInformation GetObjectsTree()
        {
            ALSymbolInformation symbol = this.CreateCopy(false);
            if ((!this.kind.IsObjectDefinition()) && (this.childSymbols != null))
            {
                for (int i = 0; i < this.childSymbols.Count; i++)
                {
                    symbol.AddChildSymbol(this.childSymbols[i].GetObjectsTree());
                }
            }
            return symbol;
        }

    }
}
