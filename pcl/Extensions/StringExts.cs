using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Common;
using Iso8601DurationHelper;
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
                s = s.Trim();
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
        internal static DateTime? ParseToDateTime(this string s, DateTimeStyles dateTimeStyle = DateTimeStyles.None)
        {
            if (s.IsValid() && DateTime
                .TryParse(s, CultureInfo.InvariantCulture, dateTimeStyle, out DateTime date))
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
                if (s.StartsWithIOC("P")) // Parse ISO 8601 format e.g Imdb Json
                {
                    if(Duration.TryParse(s, out Duration dur))
                    {
                        if(dur.Hours > 0)
                        {
                            return ((int)dur.Hours * 60) + (int)dur.Minutes;
                        }
                        else if(dur.Minutes > 0)
                        {
                            return (int)dur.Minutes;
                        }
                    }
                }
                else
                {
                    s = s.Split(' ').First().Trim();
                    return s.ParseToInt();
                }
            }
            return null;
        }
        internal static int? GetNumber(this string s)
        {
            if (s.IsValid())
            {
                var match = Regex.Match(s, "[0-9]+");
                if (match.Success)
                {
                    if (int.TryParse(match.Value, out int n))
                    {
                        return n;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Splits a string into substrings based on the strings in an array. 
        /// <br/>
        /// <list type="bullet">
        /// <item>
        /// It applies <see cref="StringSplitOptions.RemoveEmptyEntries"/> 
        /// to make sure that the return array does not include empty strings. 
        /// </item>
        /// <item>
        /// It finally trims leading and trailing whitespace characters from each item.
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="s"></param>
        /// <param name="args"></param>
        /// <returns>
        /// Substrings of the original <see cref="string"/> as pairs.
        /// </returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
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