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
        
        public ALExtensionLibraryTypeProxy(ALExtensionLibraryProxy lib, string name, params string[] genericTypeParameters)
        {
            this.Library = lib;
            this.Name = name;
            this.Type = lib.LibraryAssembly.GetType(this.Name);

            if ((genericTypeParameters != null) && (genericTypeParameters.Length > 0))
            {
                Type[] typeParameters = new Type[genericTypeParameters.Length];
                for (int i = 0; i < genericTypeParameters.Length; i++)
                {
                    typeParameters[i] = lib.LibraryAssembly.GetType(genericTypeParameters[i]);
                }
                this.Type = this.Type.MakeGenericType(typeParameters);
            }
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
                    (!parameterTypes[i].IsSubclassOf(parameterType)) &&
                    (!parameterType.IsGenericParameter))
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
                    {
                        if (method.ContainsGenericParameters)
                            method = this.MakeGenericMethod(method, parameterInfoList, parameters);
                        return method.Invoke(null, parameters);
                    }
                }
            }

            return null;
        }

        protected MethodInfo MakeGenericMethod(MethodInfo method, ParameterInfo[] methodParameters, dynamic[] parametersValues)
        {
            //collect generic parameters
            Dictionary<string, Type> paramTypes = new Dictionary<string, Type>();
            for (int i = 0; i < methodParameters.Length; i++)
            {
                if (methodParameters[i].ParameterType.IsGenericParameter)
                {
                    if (!paramTypes.ContainsKey(methodParameters[i].ParameterType.Name))
                        paramTypes.Add(methodParameters[i].ParameterType.Name, parametersValues[i].GetType());
                }
            }
            //create generic parameter types
            Type[] types = method.GetGenericArguments();
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = paramTypes[types[i].Name];
            }

            return method.MakeGenericMethod(types);
        }

        public dynamic CreateInstance(params dynamic[] values)
        {
            return Activator.CreateInstance(this.Type, values);
        }

    }
}
