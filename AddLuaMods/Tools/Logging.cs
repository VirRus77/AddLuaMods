using System;
using System.Collections.Generic;
using UnityModManagerNet;

namespace AddLuaMods.Tools
{
    public static class Logging
    {
        private static readonly Dictionary<LogLevel, string> LogLevelToString = new Dictionary<LogLevel, string>
        {
            { LogLevel.Trace, "TRC" },
            { LogLevel.Debug, "DBG" },
            { LogLevel.Information, "INF" },
            { LogLevel.Warning, "WRN" },
            { LogLevel.Error, "ERR" },
            { LogLevel.Fatal, "FTL" },
        };

        public enum LogLevel
        {
            Trace,
            Debug,
            Information,
            Warning,
            Error,
            Fatal,
        }

#if DEBUG
        public static LogLevel MinimalLevel = LogLevel.Trace;
#else
        public static LogLevel MinimalLevel = LogLevel.Error;
#endif

        public static void LogTrace(string value)
        {
            Log(LogLevel.Trace, value);
        }

        public static void LogDebug(string value)
        {
            Log(LogLevel.Debug, value);
        }

        public static void LogInfo(string value)
        {
            Log(LogLevel.Information, value);
        }

        public static void LogWarning(string value)
        {
            Log(LogLevel.Warning, value);
        }

        public static void LogError(string value)
        {
            Log(LogLevel.Error, value);
        }

        public static void LogFatal(string value)
        {
            Log(LogLevel.Fatal, value);
        }

        public static void LogException(LogLevel logLevel, Exception exception, string? value = null)
        {
            if (MinimalLevel > logLevel)
            {
                return;
            }

            UnityModManager.Logger.LogException(
                $"{DateTime.Now:yyyy.MM.dd HH:mm:ss.fff} [{LogLevelToString[logLevel]}]{(string.IsNullOrEmpty(value) ? "" : $" {value}")}",
                exception
            );
        }

        private static void Log(LogLevel logLevel, string value)
        {
            if (MinimalLevel > logLevel)
            {
                return;
            }

            UnityModManager.Logger.Log($"{DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff")} [{LogLevelToString[logLevel]}] {value}");
        }
    }
}
