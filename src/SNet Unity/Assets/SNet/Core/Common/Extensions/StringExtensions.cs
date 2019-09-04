using System;

namespace SNet.Core.Common.Extensions
{
    public static class StringExtensions
    {
        public static string AfterLast(this string str, string sub)
        {
            var idx = str.LastIndexOf(sub, StringComparison.Ordinal);
            return idx < 0 ? "" : str.Substring(idx + sub.Length);
        }

        public static string BeforeLast(this string str, string sub)
        {
            var idx = str.LastIndexOf(sub, StringComparison.Ordinal);
            return idx < 0 ? "" : str.Substring(0, idx);
        }

        public static string AfterFirst(this string str, string sub)
        {
            var idx = str.IndexOf(sub, StringComparison.Ordinal);
            return idx < 0 ? "" : str.Substring(idx + sub.Length);
        }

        public static string BeforeFirst(this string str, string sub)
        {
            var idx = str.IndexOf(sub, StringComparison.Ordinal);
            return idx < 0 ? "" : str.Substring(0, idx);
        }

        public static int PrefixMatch(this string str, string prefix)
        {
            int l = 0, slen = str.Length, plen = prefix.Length;
            while (l < slen && l < plen)
            {
                if (str[l] != prefix[l])
                    break;
                l++;
            }
            return l;
        }
    }
}
