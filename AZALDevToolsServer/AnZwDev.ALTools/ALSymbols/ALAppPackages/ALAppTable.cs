using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppTable : ALAppObject
    {


        public ALAppElementsCollection<ALAppTableField> Fields { get; set; }
        public ALAppElementsCollection<ALAppTableKey> Keys { get; set; }
        public ALAppElementsCollection<ALAppFieldGroup> FieldGroups { get; set; }

        public ALAppTable()
        {
        }

        protected override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.TableObject;
        }

        protected override void AddChildALSymbols(ALSymbolInformation symbol)
        {
            this.Fields?.AddToALSymbol(symbol, ALSymbolKind.FieldList, "fields");
            this.Keys?.AddToALSymbol(symbol, ALSymbolKind.KeyList, "keys", ALSymbolKind.PrimaryKey);
            this.FieldGroups?.AddToALSymbol(symbol, ALSymbolKind.FieldGroupList, "fieldgroups");
            base.AddChildALSymbols(symbol);
        }

    }
}
