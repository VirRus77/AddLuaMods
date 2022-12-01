using System;
using System.Linq;

namespace AddLuaMods.Tools
{
    public static class ConvertArgs
    {
        public static T Convert<T>(this object[] _args)
        {
            var values = Convert(_args, new[] { typeof(T) });

            return (T)values[0];
        }

        public static (T, T2) Convert<T, T2>(this object[] _args)
        {
            var values = Convert(_args, new[] { typeof(T), typeof(T2) });

            return ((T)values[0], (T2)values[1]);
        }

        //public static Tuple<T, T2> Convert<T, T2>(this object[] _args)
        //{
        //    var values = Convert(_args, new[] { typeof(T), typeof(T2) });

        //    return new Tuple<T, T2>((T)values[0], (T2)values[1]);
        //}

        public static Tuple<T, T2, T3> Convert<T, T2, T3>(this object[] _args)
        {
            var values = Convert(_args, new[] { typeof(T), typeof(T2), typeof(T3) });

            return new Tuple<T, T2, T3>((T)values[0], (T2)values[1], (T3)values[2]);
        }

        private static object[] Convert(object[] args, Type[] types)
        {
            if (args == null)
            {
                Logging.LogDebug("Convert args == null");
                throw new Exception("args == null");
            }

            if (args.Length != types.Length)
            {
                Logging.LogDebug($"args.Length != types.Length: {args.Length} {types.Length}");
                throw new Exception($"args.Length != types.Length: {args.Length} {types.Length}");
            }

            var zip = args
                .Zip(types, (o, type) => new Tuple<Type, Type, bool>(o.GetType(), type, o.GetType() == type || type.IsAssignableFrom(o.GetType())))
                .ToArray();
            if (zip.Any(v => !v.Item3))
            {
                var typesNotEq = string.Join(", ", zip.Where(v => !v.Item3).Select(v => $"{v.Item1} != {v.Item2}"));
                Logging.LogDebug(
                    $"args.Type error: {typesNotEq}"
                );
                throw new Exception($"args.Type error: {typesNotEq}");
            }

            return args;
        }
    }
}
