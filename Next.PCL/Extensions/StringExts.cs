using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Next.PCL.Enums;

namespace Next.PCL.Extensions
{
    internal static class StringExts
    {
        internal static bool IsNotEmptyOr(this string s, string defaultValue = "N/A")
        {
            if (s.IsValid())
            {
                return !s.EqualsOIC(defaultValue);
            }
            return false;
        }
        internal static double? ParseToDouble(this string s)
        {
            if (s.IsValid() && double.TryParse(s, out double d))
                return d;
            return null;
        }
        internal static int? ParseToInt(this string s)
        {
            if (s.IsValid())
            {
                s = s.Replace(",", "").Trim();
                if (!s.Contains('.'))
                {
                    if (int.TryParse(s, out int n))
                        return n;
                }
                else
                {
                    if (double.TryParse(s, out double d))
                        return Convert.ToInt32(d);
                }
            }
            return null;
        }
        internal static DateTime? ParseToDateTime(this string s)
        {
            if (s.IsValid() && DateTime.TryParse(s, out DateTime d))
            {
                return d;
            }
            return null;
        }
        internal static MetaImageType ParseToMetaImageType(string s)
        {
            if (s.IsValid())
            {
                if (s.Matches("poster"))
                    return MetaImageType.Poster;
                else if (s.MatchesAny("backdrop", "background"))
                    return MetaImageType.Backdrop;
                else if (s.Matches("banner"))
                    return MetaImageType.Banner;
                else if (s.Matches("typography"))
                    return MetaImageType.Typography;
            }
            return MetaImageType.Image;
        }
        internal static MetaVideoType ParseToMetaVideoType(string type)
        {
            if (type.IsValid())
            {
                if (type.EqualsOIC("trailer"))
                    return MetaVideoType.Trailer;
            }
            return MetaVideoType.Clip;
        }
        internal static IEnumerable<string> SplitByAndTrim(this string s, params string[] args)
        {
            if (s.IsValid())
            {
                return s.Split(args, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim());
            }
            return Array.Empty<string>();
        }
    }
}