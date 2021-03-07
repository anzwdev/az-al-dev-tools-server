using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.ALAppPackages
{
    public class ALAppElementsCollection<T> : List<T>, IALAppElementsCollection where T : ALAppBaseElement
    {

        public ALAppElementsCollection()
        {
        }

        public void AddBaseElement(ALAppBaseElement element)
        {
            this.Add((T)element);
        }

        public void RemoveBaseElement(ALAppBaseElement element)
        {
            this.Remove((T)element);
        }


        #region ALSymbolInformation conversion

        public void AddToALSymbol(ALSymbolInformation symbol)
        {
            this.AddToALSymbol(symbol, ALSymbolKind.Undefined, null, ALSymbolKind.Undefined);
        }

        public void AddCollectionToALSymbol(ALSymbolInformation symbol, ALSymbolKind collectionKind)
        {
            this.AddToALSymbol(symbol, collectionKind, collectionKind.ToName(), ALSymbolKind.Undefined);
        }

        public void AddToALSymbol(ALSymbolInformation symbol, ALSymbolKind collectionKind, string collectionName)
        {
            this.AddToALSymbol(symbol, collectionKind, collectionName, ALSymbolKind.Undefined);
        }

        public void AddToALSymbol(ALSymbolInformation symbol, ALSymbolKind collectionKind, string collectionName, ALSymbolKind firstItemSymbolKind)
        {
            if (this.Count > 0)
            {
                ALSymbolInformation collectionSymbol = symbol;
                if (!String.IsNullOrWhiteSpace(collectionName))
                {
                    collectionSymbol = new ALSymbolInformation(collectionKind, collectionName);
                    symbol.AddChildSymbol(collectionSymbol);
                }

                for (int i = 0; i < this.Count; i++)
                {
                    if ((i == 0) && (firstItemSymbolKind != ALSymbolKind.Undefined))
                    {
                        ALSymbolInformation itemSymbol = this[i].ToALSymbol();
                        itemSymbol.kind = firstItemSymbolKind;
                        collectionSymbol.AddChildSymbol(itemSymbol);
                    }
                    else
                        collectionSymbol.AddChildSymbol(this[i].ToALSymbol());
                }
            }
        }

        #endregion

    }
}
