using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.MergedReferences
{
    public class MergedALAppObjectExtensionsCollection<T> : MergedALAppObjectsCollection<T> where T : ALAppObject
    {

        public MergedALAppObjectExtensionsCollection(IReadOnlyList<ALAppSymbolReference> allSymbols, Func<ALAppSymbolReference, IList<T>> getALAppObjectsCollection) : base(allSymbols, getALAppObjectsCollection)
        {
        }

        protected virtual bool ExtendsObject(T alObject, string baseObjectName)
        {
            return false;
        }

        public IEnumerable<T> FindAllExtensions(string baseObjectName)
        {
            for (int objListIdx = 0; objListIdx < this.AllSymbolReferences.Count; objListIdx++)
            {
                IList<T> objectsList = this.GetALAppObjectsCollection(this.AllSymbolReferences[objListIdx]);
                if (objectsList != null)
                    for (int objIdx = 0; objIdx < objectsList.Count; objIdx++)
                        if ((objectsList[objIdx] != null) && (this.ExtendsObject(objectsList[objIdx], baseObjectName)))
                            yield return objectsList[objIdx];
            }
        }


    }
}
