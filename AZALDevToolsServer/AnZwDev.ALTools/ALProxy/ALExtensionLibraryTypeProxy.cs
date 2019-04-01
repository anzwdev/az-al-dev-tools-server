using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALProxy
{
    public class ALExtensionLibraryTypeProxy
    {

        public ALExtensionLibraryProxy Library { get; }
        public string Name { get; }
        public Type Type { get; }
        
        public ALExtensionLibraryTypeProxy(ALExtensionLibraryProxy lib, string name)
        {
            this.Library = lib;
            this.Name = name;
            this.Type = lib.LibraryAssembly.GetType(this.Name);
        }

        protected bool ValidParameterTypes(Type[] parameterTypes, ParameterInfo[] parameterInfoList)
        {
            if (parameterTypes.Length != parameterInfoList.Length)
                return false;
            for (int i=0; i<parameterTypes.Length; i++)
            {
                Type parameterType = parameterInfoList[i].ParameterType;
                if ((parameterTypes[i] != null) && 
                    (parameterTypes[i] != parameterType) &&
                    (!parameterTypes[i].IsSubclassOf(parameterType)))
                    return false;
            }
            return true;
        }

        public dynamic CallStaticMethod(string name, params dynamic[] parameters)
        {
            Type[] parameterTypes = new Type[parameters.Length];
            for (int i=0; i<parameters.Length; i++)
            {
                if ((object)parameters[i] == Type.Missing)
                    parameterTypes[i] = null;
                else
                    parameterTypes[i] = parameters[i].GetType();
            }
            MethodInfo[] methodList = this.Type.GetMethods(BindingFlags.Public | BindingFlags.Static); //parameterTypes);



            for (int i=0; i<methodList.Length; i++)
            {
                MethodInfo method = methodList[i];
                if (method.Name == name)
                {
                    ParameterInfo[] parameterInfoList = method.GetParameters();
                    if (ValidParameterTypes(parameterTypes, parameterInfoList))
                        return method.Invoke(null, parameters);
                }
            }



            return null;
        }

    }
}
