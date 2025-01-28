using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Redbox.NetCore.Logging.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogDebugWithSource(this ILogger logger, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string callerLocation = "")
        {
            logger.LogDebug(string.Concat(new string[]
            {
                callerLocation.ClassName(),
                ".",
                memberName,
                " -> ",
                message
            }), Array.Empty<object>());
        }

        public static void LogInfoWithSource(this ILogger logger, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string callerLocation = "")
        {
            logger.LogInformation(string.Concat(new string[]
            {
                callerLocation.ClassName(),
                ".",
                memberName,
                " -> ",
                message
            }), Array.Empty<object>());
        }

        public static void LogErrorWithSource(this ILogger logger, Exception ex, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string callerLocation = "")
        {
            logger.LogError(ex, string.Concat(new string[]
            {
                callerLocation.ClassName(),
                ".",
                memberName,
                " -> ",
                message
            }), Array.Empty<object>());
        }

        public static void LogErrorWithSource(this ILogger logger, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string callerLocation = "")
        {
            logger.LogError(string.Concat(new string[]
            {
                callerLocation.ClassName(),
                ".",
                memberName,
                " -> ",
                message
            }), Array.Empty<object>());
        }

        public static void LogWarningWithSource(this ILogger logger, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string callerLocation = "")
        {
            logger.LogWarning(string.Concat(new string[]
            {
                callerLocation.ClassName(),
                ".",
                memberName,
                " -> ",
                message
            }), Array.Empty<object>());
        }

        public static void LogCriticalWithSource(this ILogger logger, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string callerLocation = "")
        {
            logger.LogCritical(string.Concat(new string[]
            {
                callerLocation.ClassName(),
                ".",
                memberName,
                " -> ",
                message
            }), Array.Empty<object>());
        }

        public static void LogCriticalWithSource(this ILogger logger, Exception ex, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string callerLocation = "")
        {
            logger.LogCritical(ex, string.Concat(new string[]
            {
                callerLocation.ClassName(),
                ".",
                memberName,
                " -> ",
                message
            }), Array.Empty<object>());
        }

        static string ClassName(this string callerLocation)
        {
            if (!callerLocation.Contains("\\"))
            {
                string text = callerLocation.Split('/').LastOrDefault<string>();
                if (text == null)
                {
                    return null;
                }
                return text.TrimEnd(".cs");
            }
            else
            {
                string text2 = callerLocation.Split('\\').LastOrDefault<string>();
                if (text2 == null)
                {
                    return null;
                }
                return text2.TrimEnd(".cs");
            }
        }

        static string TrimEnd(this string value, string substring)
        {
            if (value == null || !value.EndsWith(substring))
            {
                return value;
            }
            return value.Remove(value.LastIndexOf(substring));
        }
    }
}
