using System.Diagnostics;
using System.Linq;

namespace AddLuaMods.Tools
{
    public static class DebugStack
    {
        public static string[] GetStackTrace()
        {
            // Get call stack
            var stackTrace = new StackTrace();
            // Get calling method name
            var callStack = stackTrace.GetFrames()
                .Skip(1)
                .Select(
                    v =>
                    {
                        var methodBase = v.GetMethod();
                        var @namespace = methodBase.DeclaringType.Namespace ?? "";
                        return
                            $"{(string.IsNullOrWhiteSpace(@namespace) ? "" : $"{@namespace}.")}{methodBase.DeclaringType.FullName ?? methodBase.DeclaringType.Name}.{methodBase.Name}";
                    }
                )
                .ToArray();
            return callStack;
        }
    }
}
