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
        public string name { get; set; }
        public string subtype { get; set; }
        public string fullName { get; set; }
        public ALSymbolKind kind { get; set; }
        public List<ALSymbolInformation> childSymbols { get; set; }
        public Range range { get; set; }
        public Range selectionRange { get; set; }

        public ALSymbolInformation()
        {
            this.id = 0;
            this.childSymbols = null;
            this.fullName = null;
            this.subtype = null;
        }

        public ALSymbolInformation(ALSymbolKind kindValue, string nameValue) : this()
        {
            this.kind = kindValue;
            this.name = nameValue;
            this.fullName = nameValue;
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
                this.fullName = this.kind.ToName() + " " + this.name;

            if (this.childSymbols != null)
            {
                for (int i=0; i<this.childSymbols.Count;i++)
                {
                    this.childSymbols[i].UpdateFields();
                }
            }
        }

    }
}
