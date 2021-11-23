using AnZwDev.ALTools.ALSymbolReferences;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class BaseExtendableObjectInformationProvider<T,E> : BaseObjectInformationProvider<T> where T: ALAppObject where E: ALAppObject
    {
        public BaseExtendableObjectInformationProvider()
        {
        }

        protected IEnumerable<E> FindAllExtensions(ALProject project, T alObject)
        {
            return null;
        }

        protected override IEnumerable<ALAppMethod> GetMethods(ALProject project, T alObject)
        {
            //main methods
            if ((alObject != null) && (alObject.Methods != null))
            {
                foreach (ALAppMethod alAppMethod in alObject.Methods) 
                { 
                    yield return alAppMethod; 
                }
            }

            //extension methods
            IEnumerable<E> extensions = this.FindAllExtensions(project, alObject);
            if (extensions != null)
            {
                foreach (E ext in extensions)
                {
                    if ((ext != null) && (ext.Methods != null))
                    {
                        foreach (ALAppMethod alAppMethod in ext.Methods) 
                        { 
                            yield return alAppMethod; 
                        }
                    }
                }
            }
        }

        protected override IEnumerable<ALAppVariable> GetVariables(ALProject project, T alObject)
        {
            //main methods
            if ((alObject != null) && (alObject.Variables != null))
            {
                foreach (ALAppVariable alVariable in alObject.Variables)
                {
                    yield return alVariable;
                }
            }

            //extension methods
            IEnumerable<E> extensions = this.FindAllExtensions(project, alObject);
            if (extensions != null)
            {
                foreach (E ext in extensions)
                {
                    if ((ext != null) && (ext.Variables != null))
                    {
                        foreach (ALAppVariable alVariable in ext.Variables)
                        {
                            yield return alVariable;
                        }
                    }
                }
            }
        }


    }
}
