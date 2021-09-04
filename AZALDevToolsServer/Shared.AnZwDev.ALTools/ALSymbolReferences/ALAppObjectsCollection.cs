using System;
using System.Collections.Generic;
using System.Linq;
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

        protected override T FindElement(T element)
        {
            return this.Where(p => (p.Id == element.Id)).FirstOrDefault();
        }

    }
}
