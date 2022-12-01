using System;
using System.Collections.Concurrent;
using System.Reflection;
using JetBrains.Annotations;

namespace AddLuaMods.Tools
{
    public static class CashedReflectionInfo
    {
        public class ReflectionInfo
        {
            private ConcurrentDictionary<string, MethodInfo> _methods = new ConcurrentDictionary<string, MethodInfo>();
            private readonly Type _type;

            public ReflectionInfo(Type type)
            {
                _type = type;
            }

            public MethodInfo GetMethod(string methodName)
            {
                if (!_methods.TryGetValue(methodName, out var methodInfo))
                {
                    var tempMethodInfo = FindMethod(_type, methodName);
                    if (tempMethodInfo == null)
                    {
                        throw new Exception($"Method not found: {_type.Name}.{methodName}");
                    }

                    return _methods.GetOrAdd(methodName, tempMethodInfo);
                }

                return methodInfo;
            }

            [CanBeNull]
            private static MethodInfo? FindMethod(Type type, string methodName)
            {
                return type.GetMethod(methodName) ?? type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            }
        }

        private static ConcurrentDictionary<Type, ReflectionInfo> _typeToInfo = new ConcurrentDictionary<Type, ReflectionInfo>();

        public static MethodInfo GetMethodInfo<T>(string methodName)
        {
            return GetMethodInfo(typeof(T), methodName);
        }

        public static MethodInfo GetMethodInfo(Type type, string methodName)
        {
            var reflectionInfo = _typeToInfo.GetOrAdd(type, type1 => new ReflectionInfo(type));
            return reflectionInfo.GetMethod(methodName);
        }
    }
}
