using System;
using System.Linq;
using System.Reflection;
using AddLuaMods.Tests.Tools.Extensions;

namespace AddLuaMods.Tests.Tools
{
    public static class MakeBag
    {
        [Flags]
        public enum Types
        {
            Public = 0x01,
            Private = 0x02,
            Fields = 0x10,
            Methods = 0x20,
        }

        public static string Make<T>(
            string @namespace = "LuaMethods.Bag",
            Types makeTypes = Types.Public | Types.Private | Types.Fields | Types.Methods,
            bool makeInstance = true
        )
        {
            var type = typeof(T);
            var codeWriter = new CodeWriter();
            var typeName = type.Name;
            using (codeWriter.WriteNamespace(@namespace))
            using (codeWriter.WriteClass(type.Name))
            {
                var instance = $"_{typeName.Substring(0, 1).ToLowerInvariant()}{typeName.Substring(1)}";
                if (makeInstance)
                {
                    codeWriter.WriteField(
                        BindingFlags.NonPublic,
                        $"{type.Namespace ?? "global::"}{type.Name}",
                        instance
                    );
                    codeWriter.WriteField(
                        BindingFlags.NonPublic,
                        "HarmonyLib.Traverse",
                        "_traverse"
                    );
                    codeWriter.WriteLine();
                }

                if (makeTypes.HasFlag(Types.Fields))
                {
                    WriteFields(codeWriter, type, instance, makeTypes);
                }

                if (makeTypes.HasFlag(Types.Methods))
                {
                    WriteMethods(codeWriter, type, instance, makeTypes);
                }
            }

            return codeWriter.ToString();
        }

        private static void WriteFields(CodeWriter codeWriter, Type type, string instance, Types makeType)
        {
            var publicFields = type.GetFields();
            var privateFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            if (makeType.HasFlag(Types.Public))
            {
                foreach (var field in publicFields.Where(v => v.IsStatic))
                {
                    WriteField(codeWriter, type, field, instance);
                }
            }

            if (makeType.HasFlag(Types.Private))
            {
                foreach (var field in privateFields.Where(v => v.IsStatic))
                {
                    WriteField(codeWriter, type, field, instance);
                }

                foreach (var field in privateFields.Where(v => !v.IsStatic))
                {
                    WriteField(codeWriter, type, field, instance);
                }
            }

            if (makeType.HasFlag(Types.Public))
            {
                foreach (var field in publicFields.Where(v => !v.IsStatic))
                {
                    WriteField(codeWriter, type, field, instance);
                }
            }

            //var f_m_HeartPoint = type.GetField("m_HeartPoint", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private static void WriteField(CodeWriter codeWriter, Type baseType, FieldInfo field, string instance)
        {
            if (field.IsPublic)
            {
                codeWriter.Write("public ", true);
            }
            else
            {
                codeWriter.Write("private ", true);
            }

            if (field.IsStatic)
            {
                codeWriter.Write("static ");

                var type = GetType(field.FieldType);
                codeWriter.WriteLine($"{type} {field.Name}");

                using (codeWriter.WriteBraces())
                {
                    codeWriter.WriteLine($"get => {GetType(baseType)}.{field.Name};", true);
                    codeWriter.WriteLine($"set => {GetType(baseType)}.{field.Name} = value;", true);
                }
            }
            else if (field.IsPublic)
            {
                var type = GetType(field.FieldType);
                codeWriter.WriteLine($"{type} {field.Name}");

                using (codeWriter.WriteBraces())
                {
                    codeWriter.WriteLine($"get => {instance}.{field.Name};", true);
                    codeWriter.WriteLine($"set => {instance}.{field.Name} = value;", true);
                }
            }
            else
            {
                var type = GetType(field.FieldType);
                codeWriter.WriteLine($"{type} {field.Name}");

                using (codeWriter.WriteBraces())
                {
                    codeWriter.WriteLine($"get => _traverse.Field<{type}>(\"{field.Name}\").Value;", true);
                    codeWriter.WriteLine($"set => _traverse.Field<{type}>(\"{field.Name}\").Value = value;", true);
                }
            }
        }

        private static void WriteMethods(CodeWriter codeWriter, Type type, string instance, Types makeType)
        {
            var publicMethods = type.GetMethods();
            var privateMethods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            //var anyStart = publicMethods
            //    .Where(v => v.Name.StartsWith("get_") || v.Name.StartsWith("set_"))
            //    .Concat(privateMethods.Where(v => v.Name.StartsWith("get_") || v.Name.StartsWith("set_")))
            //    .ToArray();
            publicMethods = publicMethods.Where(v => !v.IsSpecialName).ToArray();
            privateMethods = privateMethods.Where(v => !v.IsSpecialName).ToArray();

            if (makeType.HasFlag(Types.Public))
            {
                foreach (var methodInfo in publicMethods.Where(v => v.IsStatic))
                {
                    WriteMethod(codeWriter, type, methodInfo, instance);
                }
            }

            if (makeType.HasFlag(Types.Private))
            {
                foreach (var methodInfo in privateMethods.Where(v => v.IsStatic))
                {
                    WriteMethod(codeWriter, type, methodInfo, instance);
                }

                foreach (var methodInfo in privateMethods.Where(v => !v.IsStatic))
                {
                    WriteMethod(codeWriter, type, methodInfo, instance);
                }
            }

            if (makeType.HasFlag(Types.Public))
            {
                foreach (var methodInfo in publicMethods.Where(v => !v.IsStatic))
                {
                    WriteMethod(codeWriter, type, methodInfo, instance);
                }
            }

            //var f_m_HeartPoint = type.GetField("m_HeartPoint", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private static void WriteMethod(CodeWriter codeWriter, Type baseType, MethodInfo methodInfo, string instance)
        {
            if (methodInfo.IsPublic)
            {
                codeWriter.Write("public ", true);
            }
            else
            {
                codeWriter.Write("private ", true);
            }

            if (methodInfo.IsStatic)
            {
                codeWriter.Write("static ");
            }

            codeWriter.Write($"{GetType(methodInfo.ReturnType)} ");
            codeWriter.Write(methodInfo.Name);
            if (methodInfo.IsGenericMethod)
            {
                codeWriter.Write("<");
                codeWriter.Write(string.Join(", ", methodInfo.GetGenericArguments().Select(v => GetType(v))));
                codeWriter.Write(">");
            }

            codeWriter.Write("(");
            codeWriter.Write(
                string.Join(
                    ", ",
                    methodInfo.GetParameters()
                        .Select(parameterInfo => $"{GetType(parameterInfo.ParameterType)} {parameterInfo.Name}")
                )
            );
            codeWriter.WriteLine(")");
            using (codeWriter.WriteBraces())
            {
                codeWriter.Write(ident: true);
                if (methodInfo.ReturnType != typeof(void))
                {
                    codeWriter.Write("return ");
                }

                codeWriter.Write($"{instance}.{methodInfo.Name}");
                if (methodInfo.IsGenericMethod)
                {
                    codeWriter.Write("<");
                    codeWriter.Write(string.Join(", ", methodInfo.GetGenericArguments().Select(v => GetType(v))));
                    codeWriter.Write(">");
                }

                codeWriter.WriteLine($"({string.Join(", ", methodInfo.GetParameters().Select(v => v.Name))});");
            }

            codeWriter.WriteLine();
        }

        private static string GetType(Type type)
        {
            if (type == typeof(void))
            {
                return "void";
            }

            if (type.IsGenericParameter || type.IsArray)
            {
                return type.Name;
            }

            if (type.IsGenericType)
            {
                return $"{type.Name.Substring(0, type.Name.IndexOf("`"))}<" +
                       string.Join(
                           ", ",
                           type.GenericTypeArguments
                               .Select(v => GetType(v))
                       ) +
                       ">";
            }

            return FullName(type);
        }

        private static string FullName(Type type)
        {
            if (type.Namespace == null)
            {
                if (type.FullName!.Contains("+"))
                {
                    return $"global::{type.FullName.Replace("+", ".")}";
                }

                return type.FullName;
            }
            else
            {
                if (type.FullName!.Contains("+"))
                {
                    return $"{type.FullName.Replace("+", ".")}";
                }

                return $"{type.Namespace}.{type.Name}";
            }
        }
    }
}
