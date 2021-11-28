using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.MergedReferences
{
    public class MergedALAppObjectsCollection<T> : IEnumerable<T> where T : ALAppObject
    {

        protected IReadOnlyList<ALAppSymbolReference> AllSymbolReferences { get; }
        protected Func<ALAppSymbolReference, IList<T>> GetALAppObjectsCollection { get; }


        public MergedALAppObjectsCollection(IReadOnlyList<ALAppSymbolReference> allSymbolReferences, Func<ALAppSymbolReference, IList<T>> getALAppObjectsCollection)
        {
            this.AllSymbolReferences = allSymbolReferences;
            this.GetALAppObjectsCollection = getALAppObjectsCollection;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int objListIdx=0; objListIdx < this.AllSymbolReferences.Count; objListIdx++)
            {
                IList<T> objectsList = this.GetALAppObjectsCollection(this.AllSymbolReferences[objListIdx]);
                if (objectsList != null)
                    for (int objIdx=0; objIdx < objectsList.Count; objIdx++)
                        if (objectsList[objIdx] != null)
                            yield return objectsList[objIdx];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public T FindObject(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            for (int objListIdx = 0; objListIdx < this.AllSymbolReferences.Count; objListIdx++)
            {
                IList<T> objectsList = this.GetALAppObjectsCollection(this.AllSymbolReferences[objListIdx]);
                if (objectsList != null)
                    for (int objIdx = 0; objIdx < objectsList.Count; objIdx++)
                        if ((objectsList[objIdx] != null) && (name.Equals(objectsList[objIdx].Name, StringComparison.CurrentCultureIgnoreCase)))
                            return objectsList[objIdx];
            }
            return null;
        }

        public T FindObject(int id)
        {
            for (int objListIdx = 0; objListIdx < this.AllSymbolReferences.Count; objListIdx++)
            {
                IList<T> objectsList = this.GetALAppObjectsCollection(this.AllSymbolReferences[objListIdx]);
                if (objectsList != null)
                    for (int objIdx = 0; objIdx < objectsList.Count; objIdx++)
                        if ((objectsList[objIdx] != null) && (objectsList[objIdx].Id == id))
                            return objectsList[objIdx];
            }
            return null;
        }

        public T FindObject(T item)
        {
            if (item == null)
                return null;

            for (int objListIdx = 0; objListIdx < this.AllSymbolReferences.Count; objListIdx++)
            {
                IList<T> objectsList = this.GetALAppObjectsCollection(this.AllSymbolReferences[objListIdx]);
                if (objectsList != null)
                    for (int objIdx = 0; objIdx < objectsList.Count; objIdx++)
                        if ((objectsList[objIdx] != null) && (objectsList[objIdx].Id == item.Id))
                            return objectsList[objIdx];
            }

            return null;
        }

        public IEnumerable<long> GetIdsEnumerable()
        {
            for (int objListIdx = 0; objListIdx < this.AllSymbolReferences.Count; objListIdx++)
            {
                IList<T> objectsList = this.GetALAppObjectsCollection(this.AllSymbolReferences[objListIdx]);
                if (objectsList != null)
                    for (int objIdx = 0; objIdx < objectsList.Count; objIdx++)
                        if (objectsList[objIdx] != null)
                            yield return objectsList[objIdx].Id;
            }
        }

    }
}
