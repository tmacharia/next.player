using System.IO;
using AutoMapper;
using Common;
using Next.PCL.AutoMap;
using Next.PCL.Infra;
using Next.PCL.Online;
using Next.PCL.Services;
using Tests.TestModels;

namespace Tests
{
    class MocksAndSetups
    {
        private static IMapper _mapper;
        private static INaiveCache _cache;
        private static IHttpOnlineClient _httpOnlineClient;
        private static TestSettingsModel _testSettingsModel;
        private static ISearchQueryFormatter _searchQueryFormatter;

        internal static IMapper AutoMapper
        {
            get
            {
                if (_mapper == null)
                {
                    _mapper = new Mapper(AutomapperConfig.Configure());
                }
                return _mapper;
            }
        }
        internal static INaiveCache NaiveCache
        {
            get
            {
                if (_cache == null)
                {
                    var mock = new NaiveMemoryCache();
                    _cache = mock;
                }
                return _cache;
            }
        }
        internal static IHttpOnlineClient HttpOnlineClient
        {
            get
            {
                if (_httpOnlineClient == null)
                {
                    _httpOnlineClient = new HttpOnlineClient();
                }
                return _httpOnlineClient;
            }
        }
        internal static ISearchQueryFormatter SearchQueryFormatter
        {
            get
            {
                if (_searchQueryFormatter == null)
                {
                    _searchQueryFormatter = new SearchQueryFormatter();
                }
                return _searchQueryFormatter;
            }
        }

        internal static TestSettingsModel Settings
        {
            get
            {
                if (_testSettingsModel == null)
                    _testSettingsModel = ReadPrivateSettingsFile();
                return _testSettingsModel;
            }
        }


        private static TestSettingsModel ReadPrivateSettingsFile()
        {
            string json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "settings.json"));
            return json.DeserializeTo<TestSettingsModel>();
        }
    }
}