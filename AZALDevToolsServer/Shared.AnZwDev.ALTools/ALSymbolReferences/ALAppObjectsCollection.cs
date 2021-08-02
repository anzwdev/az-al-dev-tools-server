using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppObjectsCollection<T> : ALAppElementsCollection<T> where T : ALAppObject
    {

        public ALAppObjectsCollection()
        {
        }

        public IEnumerable<long> GetIdsEnumerable()
        { 
            for (int i=0; i<this.Count; i++)
            {
                yield return this[i].Id;
            }      
        }

        public IEnumerable<ALAppObject> GetALAppObjectsEnumerable()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this[i];
            }
        }

    }
}
