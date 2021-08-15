using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ObjectInformationProvider
    {

        public List<ObjectInformation> GetProjectObjects(ALProject project, HashSet<ALSymbolKind> includeObjects, bool includeDependencies)
        {
            List<ObjectInformation> objectInformationCollection = new List<ObjectInformation>();
            this.AddObjects(project.Symbols, includeObjects, objectInformationCollection);
            if ((includeDependencies) && (project.Dependencies != null))
            {
                for (int i=0; i<project.Dependencies.Count; i++)
                {
                    this.AddObjects(project.Dependencies[i].Symbols, includeObjects, objectInformationCollection);
                }
            }    
            return objectInformationCollection;
        }

        protected void AddObjects(ALAppSymbolReference appSymbolReference, HashSet<ALSymbolKind> includeObjects, List<ObjectInformation> objectInformationCollection)
        {
            if (appSymbolReference != null)
            {
                IEnumerable<ALAppObject> alAppObjectsEnumerable = appSymbolReference.GetAllALAppObjectsEnumerable(includeObjects);
                foreach (ALAppObject alAppObject in alAppObjectsEnumerable)
                {
                    objectInformationCollection.Add(new ObjectInformation(alAppObject));
                }
            }
        }

    }
}
