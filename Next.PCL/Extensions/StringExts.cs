using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Common;
using Next.PCL.Enums;
using Serilog;

namespace Next.PCL.Extensions
{
    internal static class PrimitiveExts
    {
        internal static int GetValOrDef(this int? val)
        {
            return val.GetValueOrDefault();
        }
    }
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
        internal static bool? ParseToBool(this string s)
        {
            if (s.IsValid())
            {
                if (s.EqualsOIC("true"))
                    return true;
                else if (s.EqualsOIC("false"))
                    return false;
            }
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
            if (s.IsValid() && DateTime
                .TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            return null;
        }
        internal static Uri ParseToUri(this string s, UriKind uriKind = UriKind.RelativeOrAbsolute)
        {
            if (s.IsValid())
            {
                if (Uri.TryCreate(s, uriKind, out Uri uri))
                    return uri;
                else
                    Log.Warning("Failed to parse '{0}' to Uri.", s);
            }
            return null;
        }
        internal static int? ParseToRuntime(this string s)
        {
            if (s.IsValid())
            {
                s = s.Split(' ').First().Trim();
                return s.ParseToInt();
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
        internal static Profession ParseToProfession(this string s)
        {
            if (s.IsValid())
            {
                if (s.Matches("Director"))
                    return Profession.Director;
                else if (s.Matches("Writer"))
                    return Profession.Writer;
                else if (s.Matches("Producer"))
                    return Profession.Producer;
            }
            return Profession.Other;
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