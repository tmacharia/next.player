using Common;
using System;
using System.Text.RegularExpressions;

namespace Next.PCL.Services
{
    public class SearchQueryFormatter : ISearchQueryFormatter
    {
        public QueryFormatResult CleanAndFormat(string query)
        {
            if (!query.IsValid())
                throw new ArgumentException("Invalid query term.");

            if (query.Length <= 3)
                throw new ArgumentException("Query term too short.");

            QueryFormatResult result = new QueryFormatResult();

            query = query.Trim();
            // check for year in the query string
            Match match = Regex.Match(query, "([0-9]{4})");
            if (match.Success)
            {
                int year = int.Parse(match.Value);
                if(year < 1700)
                {
                    // probably not a year
                    result.Term = query;
                }
                else
                {
                    result.Year = year;
                    string good = query.Substring(0, match.Index);
                    string bad = query.Substring(match.Index + match.Length);
                    //Console.WriteLine("Bad\t: {0}", bad);
                    result.Term = good.Trim('(', ')', '-', '.', ' ');
                    if (result.Term.Contains("."))
                        result.Term = result.Term.Replace(".", " ");
                }
            }
            //Console.WriteLine(result);
            return result;
        }
    }
}