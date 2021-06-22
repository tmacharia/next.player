using Common;
using Next.PCL.Enums;

namespace Next.PCL.Extensions
{
    internal static class EnumsExts
    {
        internal static MetaType ParseToMetaType(this string s)
        {
            if (s.IsValid())
            {
                if (s.EqualsOIC("movie"))
                    return MetaType.Movie;
                else if (s.EqualsOIC("tv,show,tvshow,series,tvseries", true))
                    return MetaType.TvShow;
            }
            return MetaType.Unknown;
        }
        internal static MetaImageType ParseToMetaImageType(this string s)
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
        internal static Gender ParseToGender(this string s)
        {
            if (s.IsValid())
            {
                if (s.EqualsOIC("Male"))
                    return Gender.Male;
                else if (s.EqualsOIC("Female"))
                    return Gender.Female;
            }
            return Gender.Unknown;
        }
        internal static MetaVideoType ParseToMetaVideoType(this string type)
        {
            if (type.IsValid())
            {
                if (type.EqualsOIC("trailer"))
                    return MetaVideoType.Trailer;
            }
            return MetaVideoType.Clip;
        }
        internal static MetaStatus ParseToMetaStatus(this string s)
        {
            if (s.IsValid())
            {
                if (s.Matches("completed"))
                    return MetaStatus.Released;
                if (s.EqualsOIC("airing,continuing", true))
                    return MetaStatus.Airing;
                if (s.EqualsOIC("end,ended", true))
                    return MetaStatus.Ended;
                if (s.EqualsOIC("production,inproduction", true))
                    return MetaStatus.InProduction;
            }
            return MetaStatus.Released;
        }
        internal static CompanyService ParseToCompanyType(this string s)
        {
            if (s.IsValid())
            {
                if (s.Matches("production"))
                    return CompanyService.Production;
            }
            return CompanyService.Network;
        }
    }
}