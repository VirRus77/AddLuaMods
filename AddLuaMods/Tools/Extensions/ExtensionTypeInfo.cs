using System;
using System.Linq;
using System.Reflection;

namespace AddLuaMods.Tools.Extensions
{
    public static class ExtensionTypeInfo
    {
        private const BindingFlags Everything = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

        public static object InvokeMethod(this TypeInfo typeInfo, object instance,string methodName, params object[] args)
        {
            var methodInfo= typeInfo.GetDeclaredMethod(methodName);
            if (methodInfo == null)
            {
                throw new Exception($"Not found method: {methodName}");
            }

            return methodInfo.Invoke(instance, args);
        }

        public static MethodInfo[] GetAllMethods(this Type type)
        {
            return type.GetMethods(Everything);
        }

        public static MethodInfo? GetMethodInfo<T>(this Type type, string methodName)
        {
            return type.GetAllMethods()
                .Where(v => v.DeclaringType == typeof(T))
                .Where(v=>v.Name == methodName)
                .SingleOrDefault();
        }

        public static MethodInfo? GetMethodInfo(this Type type, string methodName)
        {
            return type.GetAllMethods()
                .Where(v => v.DeclaringType == type)
                .Where(v => v.Name == methodName)
                .SingleOrDefault();
        }

    }
}
