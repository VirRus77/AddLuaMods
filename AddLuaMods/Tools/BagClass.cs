using System;
using System.Linq;
using System.Reflection;
using AddLuaMods.Tools.Extensions;
using HarmonyLib;

namespace AddLuaMods.Tools
{
    public abstract class BagClass
    {
        protected readonly Traverse _traverse;
        protected readonly System.Type _instanceType;
        protected readonly object _instance;

        protected BagClass(object? instance)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            _instanceType = _instance.GetType();
            _traverse = Traverse.Create(_instance);
        }

        protected object InvokeMethod(string methodName, params object[] args)
        {
            var methods = _instanceType.GetAllMethods()
                .Where(v => v.Name == methodName)
                .Where(v => v.DeclaringType == _instanceType)
                .ToArray();

            if (methods.Length == 1)
            {
                return methods[0].Invoke(_instance, args);
            }

            throw new Exception($"Methods: \"{methodName}\" {methods.Length}");
        }

        protected object InvokeMethod(string declaringTypeName, string methodName, params object[] args)
        {
            var methods = _instanceType.GetAllMethods()
                .Where(v => v.Name == methodName)
                .Where(v => v.DeclaringType.Name == declaringTypeName)
                .ToArray();

            if (methods.Length == 1)
            {
                return methods[0].Invoke(_instance, args);
            }

            throw new Exception($"Methods: \"{declaringTypeName}\" \"{methodName}\" {methods.Length}");
        }

        protected object InvokeMethod<T>(string methodName, params object[] args)
        {
            var methods = _instanceType.GetAllMethods()
                .Where(v => v.Name == methodName)
                .Where(v => v.DeclaringType == typeof(T))
                .ToArray();

            if (methods.Length == 1)
            {
                return methods[0].Invoke(_instance, args);
            }

            throw new Exception($"Methods: \"{typeof(T).FullName}\" \"{methodName}\" {methods.Length}");
        }
    }

    public abstract class BoxClass<T>
    {
        protected static readonly Traverse _traverse = Traverse.Create<T>();
        protected static readonly System.Type _instanceType = typeof(T);
        protected readonly T _instance;

        protected BoxClass(T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            _instance = instance;
        }

        protected object InvokeMethod(string methodName, params object[] args)
        {
            var methods = _instanceType.GetAllMethods()
                .Where(v => v.Name == methodName)
                .Where(v => v.DeclaringType == _instanceType)
                .ToArray();

            if (methods.Length == 1)
            {
                return methods[0].Invoke(_instance, args);
            }

            throw new Exception($"Methods: \"{methodName}\" {methods.Length}");
        }

        protected object InvokeMethod(string declaringTypeName, string methodName, params object[] args)
        {
            var methods = _instanceType.GetAllMethods()
                .Where(v => v.Name == methodName)
                .Where(v => v.DeclaringType.Name == declaringTypeName)
                .ToArray();

            if (methods.Length == 1)
            {
                return methods[0].Invoke(_instance, args);
            }

            throw new Exception($"Methods: \"{declaringTypeName}\" \"{methodName}\" {methods.Length}");
        }

        protected object InvokeMethod<T>(string methodName, params object[] args)
        {
            var methods = _instanceType.GetAllMethods()
                .Where(v => v.Name == methodName)
                .Where(v => v.DeclaringType == typeof(T))
                .ToArray();

            if (methods.Length == 1)
            {
                return methods[0].Invoke(_instance, args);
            }

            throw new Exception($"Methods: \"{typeof(T).FullName}\" \"{methodName}\" {methods.Length}");
        }

        protected void SetField<TValue>(string fieldName, TValue value, bool isStatic = false)
        {
            var fields = _instanceType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                .Where(v => v.Name == fieldName)
                .Where(v => v.FieldType == typeof(TValue))
                .ToArray();
            if (fields.Length == 1)
            {
                if (!isStatic)
                {
                    fields.Single().SetValue(_instance, value);
                }
                else
                {
                    fields.Single().SetValue(null, value);
                }

                return;
            }

            throw new Exception($"SetField: \"{typeof(TValue).FullName}\" \"{fieldName}\" {fields.Length}");
        }

        protected TValue GetField<TValue>(string fieldName, bool isStatic = false)
        {
            var fields = _instanceType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                .Where(v => v.Name == fieldName)
                .Where(v => v.FieldType == typeof(TValue))
                .ToArray();
            if (fields.Length == 1)
            {
                if (!isStatic)
                {
                    return (TValue)fields.Single().GetValue(_instance);
                }
                else
                {
                    return (TValue)fields.Single().GetValue(null);
                }
            }

            throw new Exception($"GetField: \"{typeof(T).FullName}\" \"{fieldName}\" {fields.Length}");
        }
    }
}
