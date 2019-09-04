using System;
using System.Collections.Generic;

namespace SNet.Core.Common.Extensions
{
    public static class CommandLineArgumentExtensions
    {
        public static T ArgumentForOption<T>(this IList<string> args, string option, T defaultArgument = default)
        {
            var idx = args.IndexOf(option);
            if (idx < 0)
                return defaultArgument;
            return idx < args.Count - 1 ? (T) Convert.ChangeType(args[idx + 1], typeof(T)) : defaultArgument;
        }

        public static string ArgumentForOption(this IList<string> args, string option, string defaultArgument = "")
        {
            return args.ArgumentForOption<string>(option, defaultArgument);
        }

        public static bool OptionExists(this IList<string> args, string option)
        {
            var idx = args.IndexOf(option);
            return idx >= 0;
        }
    }
}