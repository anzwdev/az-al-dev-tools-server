using AnZwDev.ALTools.ALSymbolReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class BaseObjectInformationProvider<T> where T : ALAppObject
    {

        public BaseObjectInformationProvider()
        {
        }

        #region Objects list and search

        protected virtual ALAppObjectsCollection<T> GetALAppObjectList(ALAppSymbolReference symbols)
        {
            return null;
        }

        protected IEnumerable<T> GetAllALAppObjects(ALProject project)
        {
            return null;
        }

        protected T FindALAppObject(ALProject project, string name)
        {
            IEnumerable<T> allObjects = this.GetAllALAppObjects(project);
            if (allObjects == null)
                return null;
            return allObjects
                .Where(p => (name.Equals(p.Name, StringComparison.CurrentCultureIgnoreCase)))
                .FirstOrDefault();
        }

        protected T FindALAppObject(ALProject project, int id)
        {
            IEnumerable<T> allObjects = this.GetAllALAppObjects(project);
            if (allObjects == null)
                return null;
            return allObjects
                .Where(p => (p.Id == id))
                .FirstOrDefault();
        }

        #endregion

        #region Object methods

        protected virtual IEnumerable<ALAppMethod> GetMethods(ALProject project, T alObject)
        {
            return alObject?.Methods;
        }

        public IEnumerable<ALAppMethod> GetMethods(ALProject project, string name)
        {
            return this.GetMethods(project, this.FindALAppObject(project, name));
        }

        public IEnumerable<ALAppMethod> GetMethods(ALProject project, int id)
        {
            return this.GetMethods(project, this.FindALAppObject(project, id));
        }

        #endregion

        #region Object variables

        protected virtual IEnumerable<ALAppVariable> GetVariables(ALProject project, T alObject)
        {
            return alObject?.Variables;
        }

        public IEnumerable<ALAppVariable> GetVariables(ALProject project, string name)
        {
            return this.GetVariables(project, this.FindALAppObject(project, name));
        }

        public IEnumerable<ALAppVariable> GetVariables(ALProject project, int id)
        {
            return this.GetVariables(project, this.FindALAppObject(project, id));
        }

        #endregion

    }
}
