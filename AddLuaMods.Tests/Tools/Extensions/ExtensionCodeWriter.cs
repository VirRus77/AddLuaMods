using System;
using System.Linq;
using System.Reflection;

namespace AddLuaMods.Tests.Tools.Extensions
{
    public static class ExtensionCodeWriter
    {
        public static DisposableBag<CodeWriter> WriteBraces(this CodeWriter codeWriter)
        {
            codeWriter.WriteLine("{", true);
            codeWriter.IncrementIdent();
            return new DisposableBag<CodeWriter>(
                codeWriter,
                writer =>
                {
                    writer.DecrementIdent();
                    writer.WriteLine("}", true);
                }
            );
        }

        public static DisposableBag<CodeWriter> WriteBracesText(this CodeWriter codeWriter, string lineText)
        {
            codeWriter.WriteLine($"{lineText}", true);
            return codeWriter.WriteBraces();
        }

        public static DisposableBag<CodeWriter> WriteNamespace(this CodeWriter codeWriter, string name)
        {
            return codeWriter.WriteBracesText($"namespace {name}");
        }

        public static DisposableBag<CodeWriter> WriteClass(this CodeWriter codeWriter, string name)
        {
            return codeWriter.WriteBracesText($"class {name}");
        }

        public static void WriteField(this CodeWriter codeWriter, BindingFlags bindingFlags, string type, string name)
        {
            if (bindingFlags.HasFlag(BindingFlags.Public))
            {
                codeWriter.Write($"public ", true);
            }
            else if (bindingFlags.HasFlag(BindingFlags.NonPublic))
            {
                codeWriter.Write($"private ", true);
            }

            codeWriter.WriteLine($"{type} {name};");
        }

        public static void WriteSummary(this CodeWriter codeWriter, string text)
        {
            codeWriter.WriteLine("/// <summary>", true);
            var lines = text.Trim()
                .Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();
            lines.ForEach(line => codeWriter.WriteLine($"/// {line}", true));

            codeWriter.WriteLine("/// </summary>", true);
        }

        public static void WriteParam(this CodeWriter codeWriter, string name, string description)
        {
            var lines = description.Trim()
                .Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();
            if (lines.Length <= 1)
            {
                codeWriter.WriteLine($"/// <param name=\"{name}\">{lines.SingleOrDefault()}</param>", true);
            }
            else
            {
                codeWriter.WriteLine($"/// <param name=\"{name}\">", true);
                lines.ForEach(line => codeWriter.WriteLine($"/// {line}", true));
                codeWriter.WriteLine($"/// </param>", true);
            }
        }

        public static void WriteReturn(this CodeWriter codeWriter, string description)
        {
            var lines = description.Trim()
                .Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();
            if (lines.Length <= 1)
            {
                codeWriter.WriteLine($"/// <return>{lines.SingleOrDefault()}</return>", true);
            }
            else
            {
                codeWriter.WriteLine($"/// <return>", true);
                lines.ForEach(line => codeWriter.WriteLine($"/// {line}", true));
                codeWriter.WriteLine($"/// </return>", true);
            }
        }
    }
}
