using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using HtmlAgilityPack;
using Next.PCL.AutoMap;
using Next.PCL.Configurations;
using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Extensions;
using Next.PCL.Metas;
using Next.PCL.Online.Models.Tvdb;
using Next.PCL.Static;

namespace Next.PCL.Html
{
    internal class TvDbParser : BaseParser
    {
        internal const string IMAGE_EXTENSION = ".png";
        internal const string ACTOR_AVATAR_IMG = "/images/missing/actor.jpg";
        internal const string MOVIE_AVATAR_IMG = "/images/missing/movie.jpg";

        private readonly IMapper _mapper;

        internal TvdbConfig Config { get; set; }

        public TvDbParser(TvdbConfig tvdbConfig = default, IMapper autoMapper = default)
        {
            Config = tvdbConfig ?? new TvdbConfig();
            _mapper = autoMapper ?? new Mapper(AutomapperConfig.Configure());
        }

        internal TvDbShow ParseShow(string html, Uri showUrl)
        {
            var doc = ConvertToHtmlDoc(html);
            var model = _mapper.Map<TvDbShow>(ParseCommonModel(html, showUrl, doc));
            
            var lists = doc.FindAll("//div[@id='series_basic_info']/ul/li");

            model.AirsOn = GetAsText(lists, TvDbKeys.Airs).ParseToAirShedule();
            model.Runtime = GetNonDeterministicRuntime(GetNodesAsText(lists, TvDbKeys.Runtimes));
            model.Networks = GetLinks(lists, TvDbKeys.Networks).Select(x => x.ParseToCompany()).ToList();

            model.Seasons = doc.FindAll("//div[@role='tabpanel']").FirstContainingClass("tab-official")
                               .ExtendFindAll("/ul/li").WhereHasAttrib("data-number")
                               .Select(x => ParseSimpleSeason(x))
                               .Where(x => x != null).OrderBy(x => x.Number)
                               .ToList();

            return model;
        }
        internal TvDbMovie ParseMovie(string html, Uri movieUrl)
        {
            var doc = ConvertToHtmlDoc(html);
            var model = _mapper.Map<TvDbMovie>(ParseCommonModel(html, movieUrl, doc));

            var lists = doc.FindAll("//div[@id='series_basic_info']/ul/li");

            model.Runtime = GetAsText(lists, TvDbKeys.Runtime)?.ParseToRuntime();
            model.Studios = GetLinks(lists, TvDbKeys.Studio).Select(x => x.ParseToCompany()).ToList();
            model.Networks = GetLinks(lists, TvDbKeys.Networks).Select(x => x.ParseToCompany()).ToList();
            model.Distributors = GetLinks(lists, TvDbKeys.Distributor).Select(x => x.ParseToCompany()).ToList();
            var a = GetNodes(lists, TvDbKeys.Released).FirstOrDefault().LastChild;
            Console.WriteLine(a.ParseText());
            model.ReleaseDate = GetNodes(lists, TvDbKeys.Released).FirstOrDefault().LastChild.ParseDateTime();
            model.ProductionCompanies = GetLinks(lists, TvDbKeys.ProductionCompanies).Select(x => x.ParseToCompany()).ToList();

            return model;
        }
        internal TvdbModel ParseCommonModel(string html, Uri uri, HtmlDocument doc = default)
        {
            doc = doc ?? ConvertToHtmlDoc(html);
            var model = new TvdbModel
            {
                Url = uri,
                Name = doc.GetElementbyId("series_title").ParseText(),
                Plot = doc.FindAll("//div[@class='change_translation_text']")
                           .FirstWithAttrib("data-language", Config.Language)
                           ?.ParseText(),
            };

            model.Icons = doc.GetArtworksOfType(MetaImageType.Icon);
            model.Banners = doc.GetArtworksOfType(MetaImageType.Banner);
            model.Posters = doc.GetArtworksOfType(MetaImageType.Poster);
            model.Backdrops = doc.GetArtworksOfType(MetaImageType.Backdrop);

            var lists = doc.FindAll("//div[@id='series_basic_info']/ul/li");

            model.Id = GetAsText(lists, TvDbKeys.TvDBID).ParseToInt() ?? 0;
            model.Genres = GetNodesAsText(lists, TvDbKeys.Genres);
            model.Settings = GetNodesAsText(lists, TvDbKeys.Setting);
            model.TimePeriods = GetNodesAsText(lists, TvDbKeys.TimePeriod);
            model.Status = GetAsText(lists, TvDbKeys.Status).ParseToMetaStatus();
            model.Locations = GetNodes(lists, TvDbKeys.Location).Select(x => x.ParseText().ToGeoLocale()).ToList();

            model.OtherSites = GetLinks(lists, TvDbKeys.OtherSites).Select(x => x.ParseToMetaUrl()).ToList();
            model.Trailers = GetLinks(lists, TvDbKeys.Trailers).Select(x => x.ParseToMetaVideo()).ToList();

            return model;
        }

        public Company ParseCompany(string html, Uri companyUrl)
        {
            var doc = ConvertToHtmlDoc(html);
            var model = new Company();

            var lists = doc.FindAll("//ul[@class='list-group']/li");
            var img = doc.FindByTag("img").FirstWithAttribContaining("src", "/icons/");

            model.Id = GetAsText(lists, TvDbKeys.ID).ParseToInt() ?? 0;
            model.Name = doc.GetElementbyId("series_title").ParseText();
            model.Address = GetAsText(lists, TvDbKeys.Country);
            model.Service = GetAsText(lists, TvDbKeys.PrimaryType).ParseToCompanyType();
            model.Urls.Add(companyUrl.ParseToMetaUrl(MetaSource.TVDB));
            model.Images.AddRange(img.ParseToImagesAs(MetaImageType.Logo));

            return model;
        }
        public IEnumerable<TinyTvdbModel> ParseCompanyMedias(string html, MetaType metaType)
        {
            var doc = ConvertToHtmlDoc(html);

            string metaName = metaType == MetaType.Movie ? "Movies"
                            : metaType == MetaType.TvShow ? "Series" : "";

            var list = doc.FindByTag("h3")
                          .WhereTextEquals(metaName)
                          .GetAdjacent("div")
                          .Elements("div");

            foreach (var item in list)
            {
                var model = new TinyTvdbModel
                {
                    Url = string.Format("{0}{1}", SiteUrls.TVDB, item.Element("a").GetHref()).ParseToUri(),
                    Name = item.Element("cite").ParseText(),
                    Posters = item.Element("a").Element("img")
                                    .ParseToImagesAs(MetaImageType.Poster)
                                    .Where(x => x != null)
                                    .ToList()
                };
                if (Config.IgnoreMediasWithNoImages)
                {
                    if (model.Posters.All(x => x.Url != null))
                        yield return model;
                }
                else
                {
                    yield return model;
                }
            }
        }

        internal int? GetNonDeterministicRuntime(IEnumerable<string> list)
        {
            var runs = list.Select(x => x.SplitByAndTrim(" ").FirstOrDefault())
                           .Select(x => x.ParseToInt())
                           .Where(x => x.HasValue)
                           .Select(x => x.Value)
                           .ToList();

            return runs.Median();
        }

        public TvdbSeason ParseSimpleSeason(HtmlNode node)
        {
            int? num = node.GetAttrib("data-number").ParseToInt();

            if (!num.HasValue)
                return null;

            if (num <= 0 && !Config.TvShowSpecials)
                return null;

            var linkNode = node.ExtendFind("/h4/a");
            var episodesCount = node.Element("span")?.ParseInt();
            var dates = node.Element("p").ParseText().SplitByAndTrim("-");

            if (!episodesCount.HasValue)
                return null;

            if (episodesCount <= 0 && !Config.TvShowSeasonsWithNoEpisodes)
                return null;

            var model = new TvdbSeason
            {
                Number = num,
                Name = linkNode?.ParseText(),
                Url = linkNode?.GetHref()?.ParseToUri(),
                AirDate = dates.FirstOrDefault()?.ParseToDateTime(),
                LastAirDate = dates.Skip(1).FirstOrDefault()?.ParseToDateTime()
            };

            return model;
        }
        public TvdbSeason ParseSeason(string html, Uri seasonUrl)
        {
            var doc = ConvertToHtmlDoc(html);
            var model = new TvdbSeason
            {
                Url = seasonUrl,
                Name = doc.Find("//h1[@class='translated_title']").ParseText(),
                Plot = doc.FindAll("//div[@class='change_translation_text']")
                           .FirstWithAttrib("data-language", Config.Language)?.ParseText(),
            };
            model.Id = doc.GetElementbyId("season_deleted_reason_confirm")
                       ?.GetAttrib("data-id")?.ParseToInt() ?? 0;

            model.Episodes = ParseSeasonEpisodes(null, doc).ToList();
            model.AirDate = model.Episodes.FirstOrDefault()?.AirDate;
            model.LastAirDate = model.Episodes.LastOrDefault()?.AirDate;
            model.Posters = doc.GetArtworksOfType(MetaImageType.Poster);

            return model;
        }
        
        public IEnumerable<TvdbEpisode> ParseSeasonEpisodes(string html, HtmlDocument document = default)
        {
            var doc = document ?? ConvertToHtmlDoc(html);

            var rows = doc.FindAll("//table/tbody/tr");

            int index = 0;
            foreach (var row in rows)
            {
                var tds = row.Elements("td").ToArray();
                var link = tds[1].Element("a");
                var href = link.GetHref();
                index++;

                TvdbEpisode model = new TvdbEpisode
                {
                    Number = index,
                    Name = link.ParseText(),
                    Notation = tds[0].ParseText(),
                    Url = (SiteUrls.TVDB + href).ParseToUri(),
                    Runtime = tds[3].ParseText().ParseToRuntime(),
                    AirDate = tds[2].Element("div").ParseDateTime(),
                    Id = href.SplitByAndTrim("/")
                             .Last()
                             .ParseToInt()
                             .GetValueOrDefault()
                };
                yield return model;
            }
        }
        public TvdbEpisode ParseEpisode(string html, Uri episodeUrl)
        {
            var doc = ConvertToHtmlDoc(html);
            var model = new TvdbEpisode
            {
                Url = episodeUrl,
                Name = doc.Find("//h1[@class='translated_title']").ParseText(),
                Plot = doc.FindAll("//div[@class='change_translation_text']")
                           .FirstWithAttrib("data-language", Config.Language)
                           ?.Element("p")?.ParseText()
            };
            model.Runtime = doc.FindAll("//ul/li/strong").WhereTextEquals("runtime")
                         ?.ParentNode.Element("span").ParseText().ParseToRuntime();
            model.AirDate = doc.FindAll("//ul/li/strong").WhereTextContains("aired")
                         ?.ParentNode.SelectSingleNode("//span/a")?.ParseDateTime();
            model.Id = doc.GetElementbyId("episode_deleted_reason_confirm")
                         ?.GetAttrib("data-id").ParseToInt() ?? 0;

            model.Images = doc.GetArtworksOfType(MetaImageType.Image);

            var rows = doc.FindAll("//table/tbody/tr");
            foreach (var row in rows)
            {
                HtmlNode[] tds = row.Elements("td").ToArray();
                HtmlNode link = tds[0].Element("a");
                string href = link.GetHref();
                Uri url = (SiteUrls.TVDB + href).ParseToUri();
                int id = href.SplitByAndTrim("/").Last().ParseToInt() ?? 0;

                var person = new TvdbPerson
                {
                    Id = id,
                    Url = url,
                    Name = link.ParseText()
                };
                string type = tds[1].ParseText();
                var prf = type.ParseToProfession();
                if(prf != Profession.Other)
                    model.Crews.Add(new TvdbCrew(person) { Profession = prf });
                else
                {
                    string role = tds[2].ParseText();
                    model.Guests.Add(new TvdbCast(person) { Role = role.IsValid() ? role : type });
                }
            }

            return model;
        }

        internal IEnumerable<TvdbPerson> ParseCast(string html, HtmlDocument document = default)
        {
            var doc = document ?? ConvertToHtmlDoc(html);

            var nodes = doc.FindAll("//div[@class='thumbnail']");

            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    var cr = ParseSingleCast(item);
                    if (cr != null)
                        yield return cr;
                }
            }
        }
        internal IEnumerable<TvdbCrew> ParseCrew(string html, HtmlDocument document = default)
        {
            var doc = document ?? ConvertToHtmlDoc(html);

            var writers = doc.FindByTag("//h2|//h3").WhereTextEquals(TvDbKeys.Writers)
                            ?.GetAdjacent("table")?.ExtendFindAll("tr/td/a");

            var directors = doc.FindByTag("//h2|//h3").WhereTextEquals(TvDbKeys.Directors)
                            ?.GetAdjacent("table")?.ExtendFindAll("tr/td/a");

            var producers = doc.FindByTag("//h2|//h3").WhereTextEquals(TvDbKeys.Producers)
                            ?.GetAdjacent("table")?.ExtendFindAll("tr/td/a");

            return ParseAllCrew(directors, Profession.Director)
             .Concat(ParseAllCrew(writers, Profession.Writer))
             .Concat(ParseAllCrew(producers, Profession.Producer));
        }
        private IEnumerable<TvdbCrew> ParseAllCrew(HtmlNodeCollection nodes, Profession profession)
        {
            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    var cr = ParseSingleCrew(item, profession);
                    if (cr != null)
                        yield return cr;
                }
            }
        }
        private TvdbCrew ParseSingleCrew(HtmlNode node, Profession profession)
        {
            string href = node.GetHref();

            if (!href.IsValid())
                return null;

            return new TvdbCrew(profession)
            {
                Name = node.ParseText(),
                Url = (SiteUrls.TVDB + href).ParseToUri(),
                Id = href.SplitByAndTrim("/").Last().ParseToInt() ?? 0,
            };
        }
        private TvdbPerson ParseSingleCast(HtmlNode node)
        {
            string name = node.Element("h3")?.Element("#text").ParseText();
            string character = node.Element("h3")?.Element("small").ParseText().Substring(2).Trim();
            string imgUrl = node.Element("img").GetAttrib("src");

            if (!imgUrl.IsValid())
                return null;

            var cast = new TvdbPerson();
            cast.Name = name;
            cast.Role = character;

            if (imgUrl.EndsWith(ACTOR_AVATAR_IMG))
            {
                if (!Config.IgnoreActorsWithNoImages)
                    return null;

                // fallback and use the parent node element to
                // get the url for the person.
                if (node.ParentNode.Is("a"))
                {
                    cast.Url = string.Format("{0}{1}", SiteUrls.TVDB, node.ParentNode.GetHref()).ParseToUri();
                }
            }
            else
            {
                var vs = imgUrl.Split('/');
                cast.Id = vs.ElementAt(vs.Length - 2).ParseToInt() ?? 0;
                cast.Url = string.Format("{0}/people/{1}", SiteUrls.TVDB, cast.Id).ParseToUri();
                cast.Images.Add(imgUrl.ParseToUri().CreateImage(MetaImageType.Profile));
            }
            
            return cast;
        }

        internal async Task<List<MetaImage>> GetAndParseImagesAsync(Uri uri, MetaImageType type, CancellationToken token = default)
        {
            string imageType = TvdbExts.CastImageType(type);
            var url = string.Format("{0}/artwork/{1}", uri.OriginalString.TrimEnd('/'), imageType).ParseToUri();
            var doc = await GetHtmlDocumentAsync(url, token);
            return doc.GetArtworksOfType(type);
        }

        private string GetAsText(HtmlNodeCollection nodes, string name)
        {
            return nodes.FirstOrDefault(x
                            => x.Element("strong")
                                .ParseText()
                                .Matches(name))
                        ?.Element("span")
                        ?.ParseText();
        }
        private List<string> GetNodesAsText(HtmlNodeCollection nodes, string name)
        {
            return GetNodes(nodes, name).Select(x => x.ParseText()).ToList();
        }
        private IEnumerable<HtmlNode> GetLinks(HtmlNodeCollection nodes, string name)
        {
            return GetNodes(nodes, name, "a");
        }
        private IEnumerable<HtmlNode> GetNodes(HtmlNodeCollection nodes, string name, string xPath = default)
        {
            var elem = nodes.FirstOrDefault(x => x.Element("strong").ParseText().Matches(name));

            if (elem == null)
                return Array.Empty<HtmlNode>();

            string path = "span";
            if (xPath.IsValid())
            {
                if (!xPath.StartsWith("/"))
                    xPath = '/' + xPath;
                path += xPath;
            }
            return elem.ExtendFindAll(path);
        }
    }
}