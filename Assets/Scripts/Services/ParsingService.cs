using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ParsingService : MonoBehaviour
{
    // Parameter-Entity Lists
    public List<ParameterModel> dbPediaPlatforms = new();
    public List<ParameterModel> dbPediaGenres = new();
    public List<ParameterModel> dbPediaModes = new();
    public List<ParameterModel> wikiDataRatings = new();
    public List<ParameterModel> wikiDataCountries = new();

    // Query Result Lists
    public List<RankingModel> dbRankingData = new();
    public List<string> wikiDataRankingData = new();

    // Query Searching Conditions
    private bool searchDbPedia = false;
    private bool searchWikiData = false;

    // Script References
    public APIController apiController;

    // Obtain and parse parameter query results into usable Parameters
    public void ParseParameters()
    {
        try
        {
            ParseParameterResults(apiController.dbPediaComputingPlatformResult, dbPediaPlatforms);
            ParseParameterResults(apiController.dbPediaGenreResult, dbPediaGenres);
            ParseParameterResults(apiController.dbPediaModeResult, dbPediaModes);
            ParseParameterResults(apiController.wikiDataRatingResult, wikiDataRatings);
            ParseParameterResults(apiController.wikiDataCountriesResult, wikiDataCountries);
        }
        catch(Exception){}

        CleanParameters();
    }

    // Parse String Results into List of Parameters
    public void ParseParameterResults(string response, List<ParameterModel> dataList)
    {
        // Parse response into JSON
        JObject search = JObject.Parse(response);

        // Get JSON result objects into a list
        List<JToken> results = search["results"]["bindings"].Children().ToList();

        // Serialize JSON results into .NET objects
        foreach (JToken result in results)
        {
            string resourceValue = result["resource"]["value"].ToString();
            string nameValue = result["name"]["value"].ToString();

            ParameterModel parameterData = new() { Name = nameValue, Url = resourceValue };
            dataList.Add(parameterData);
        }
    }

    // Parse String Results into List of Ranks
    public void ParseQueryResults(string response, List<RankingModel> dataList)
    {
        // Parse response into JSON
        JObject search = JObject.Parse(response);

        // Get JSON result objects into a list
        List<JToken> results = search["results"]["bindings"].Children().ToList();

        // Serialize JSON results into .NET objects
        foreach (JToken result in results)
        {
            string resourceValue = result["game"]["value"].ToString();
            string nameValue = result["name"]["value"].ToString();
            string seriesValue = result["series"]["value"].ToString();
            string publisherValue = result["publisher"]["value"].ToString();
            string releaseDateValue = result["releaseDate"]["value"].ToString();
            string ignValue = result["ign"]["value"].ToString();

            RankingModel parameterData = new(resourceValue, nameValue, seriesValue, publisherValue, releaseDateValue, ignValue);
            dataList.Add(parameterData);
        }
    }

    // Parse String Results into List of Videogame Names
    public void ParseWikiDataNames(string response, List<string> dataList)
    {
        // Parse response into JSON
        JObject search = JObject.Parse(response);

        // Get JSON result objects into a list
        List<JToken> results = search["results"]["bindings"].Children().ToList();

        // Serialize JSON results into .NET objects
        foreach (JToken result in results)
        {
            string nameValue = result["name"]["value"].ToString();

            dataList.Add(nameValue);
        }
    }

    // Transform Parameters into useful format
    private void CleanParameters()
    {
        // Eliminate Non-Resources
        dbPediaPlatforms = EliminateDBPediaNonResources(dbPediaPlatforms);
        dbPediaModes = EliminateDBPediaNonResources(dbPediaModes);
        dbPediaGenres = EliminateDBPediaNonResources(dbPediaGenres);

        // Modify Names
        ModifyEntityName(dbPediaGenres);
        ModifyEntityName(dbPediaModes);

        // Eliminate Scores
        dbPediaPlatforms = EliminateScores(dbPediaPlatforms);
        dbPediaModes = EliminateScores(dbPediaModes);
        dbPediaGenres = EliminateScores(dbPediaGenres);
        wikiDataRatings = EliminateScores(wikiDataRatings);
        wikiDataCountries = EliminateScores(wikiDataCountries);

        // Eliminate Duplicates By Name
        dbPediaPlatforms = EliminateDuplicatesByName(dbPediaPlatforms);
        dbPediaModes = EliminateDuplicatesByName(dbPediaModes);
        dbPediaGenres = EliminateDuplicatesByName(dbPediaGenres);
        wikiDataRatings = EliminateDuplicatesByName(wikiDataRatings);
        wikiDataCountries = EliminateDuplicatesByName(wikiDataCountries);

        // Eliminate Duplicates By Url
        dbPediaPlatforms = EliminateDuplicatesByUrl(dbPediaPlatforms);
        dbPediaModes = EliminateDuplicatesByUrl(dbPediaModes);
        dbPediaGenres = EliminateDuplicatesByUrl(dbPediaGenres);
        wikiDataRatings = EliminateDuplicatesByUrl(wikiDataRatings);
        wikiDataCountries = EliminateDuplicatesByUrl(wikiDataCountries);

        // Eliminate Empty Names
        dbPediaPlatforms = EliminateEmptyNames(dbPediaPlatforms);
        dbPediaModes = EliminateEmptyNames(dbPediaModes);
        dbPediaGenres = EliminateEmptyNames(dbPediaGenres);

        // Order Parameters
        dbPediaPlatforms = OrderByName(dbPediaPlatforms);
        dbPediaGenres = OrderByName(dbPediaGenres);
        dbPediaModes = OrderByName(dbPediaModes);
        wikiDataRatings = OrderByName(wikiDataRatings);
        wikiDataCountries = OrderByName(wikiDataCountries);
    }

    #region Cleaning Functions

    private List<ParameterModel> OrderByName(List<ParameterModel> dataList)
    {
        return dataList.OrderBy(x => x.Name).ToList();
    }

    private List<ParameterModel> EliminateDuplicatesByUrl(List<ParameterModel> dataList)
    {
        return dataList.GroupBy(x => x.Url).Select(y => y.First()).ToList();
    }

    private List<ParameterModel> EliminateDuplicatesByName(List<ParameterModel> dataList)
    {
        dataList = dataList.GroupBy(x => x.Name).Select(y => y.First()).ToList();
        dataList = (from data in dataList where !data.Name.Contains("/") select data).ToList();
        return (from data in dataList where (data.Name.Length > 2) select data).ToList();
    }

    private List<ParameterModel> EliminateEmptyNames(List<ParameterModel> dataList)
    {
        return (from data in dataList where !string.IsNullOrEmpty(data.Name) select data).ToList();
    }

    private List<ParameterModel> EliminateDBPediaNonResources(List<ParameterModel> dataList)
    {
        string pattern = @"^http://dbpedia.org/resource/";
        return dataList.Where(entity => Regex.IsMatch(entity.Url, pattern)).ToList();
    }

    private List<ParameterModel> EliminateScores(List<ParameterModel> dataList)
    {
        foreach (ParameterModel data in dataList)
        {
            data.Name = data.Name.Replace("_", " ");
            data.Name = data.Name.Replace("-", " ");
            data.Name = data.Name.Replace("\"", " ");
            data.Name = data.Name.Trim();
        }

        return dataList;
    }

    private List<ParameterModel> ModifyEntityName(List<ParameterModel> dataList)
    {
        foreach (ParameterModel data in dataList)
        {
            data.Name = data.Url.Split('/').Last();
        }

        return dataList;
    }

    #endregion

    // Empty Query Results
    public void CleanQueryResults()
    {
        dbRankingData.Clear();
        wikiDataRankingData.Clear();

        searchDbPedia = false;
        searchWikiData = false;
    }

    // Transform Ranking Query Results into useful format
    private List<RankingModel> CleanRankingData(List<RankingModel> rankingDataList)
    {
        foreach (RankingModel data in rankingDataList)
        {
            try
            {
                data.ReleaseDate = data.ReleaseDate.Substring(0, 4);
                data.Publisher = data.Publisher.Split('/').Last().Replace("_", " ");
                data.ScoreString = Regex.Match(data.ScoreString, @"\d+").Value;
                data.Score = RangeScore(int.Parse(data.ScoreString));
            }
            catch (Exception)
            {
                continue;
            }
        }

        rankingDataList = rankingDataList.GroupBy(x => x.Resource).Select(y => y.First()).ToList();
        rankingDataList = (from data in rankingDataList where !data.Series.Contains("/") select data).ToList();
        rankingDataList = (from data in rankingDataList where (data.Name.Length > 2) select data).ToList();

        return rankingDataList;
    }

    // Maintain Score Value between 1 - 10
    private int RangeScore(int score)
    {
        if (score > 10)
        {
            score = 10;
        }
        else if (score < 1)
        {
            score = 1;
        }

        return score;
    }

    // Transform Ranking Query Names into useful format
    private List<string> CleanRankingData(List<string> wikiDataRankingList)
    {
        wikiDataRankingList = wikiDataRankingData.GroupBy(x => x).Select(y => y.First()).ToList();
        wikiDataRankingList = (from data in wikiDataRankingData where (data.Length > 2) select data).ToList();

        return wikiDataRankingList;
    }

    // Build DbPedia Query
    private string DBSearchQuery(QueryModel queryModel)
    {
        switch (queryModel.Type)
        {
            case "Platform":
                searchDbPedia = true;
                return " ?game dbo:computingPlatform <" + queryModel.Url + ">.";
            case "Mode":
                searchDbPedia = true;
                return " ?game dbp:modes <" + queryModel.Url + ">.";
            case "Genre":
                searchDbPedia = true;
                return " ?game dbo:genre <" + queryModel.Url + ">.";
            case "DateMin":
                searchDbPedia = true;
                return " ?game dbo:releaseDate ?releaseDate. FILTER (?releaseDate > \"" + queryModel.Url + "\"^^xsd:date).";
            case "DateMax":
                searchDbPedia = true;
                return " ?game dbo:releaseDate ?releaseDate. FILTER (?releaseDate < \"" + queryModel.Url + "\"^^xsd:date).";
            default:
                return "";
        }
    }

    // Build WikiData Query
    private string WikiDataSearchQuery(QueryModel queryModel)
    {
        switch (queryModel.Type)
        {
            case "Rating":
                searchWikiData = true;
                return " ?game wdt:P852 " + TransformWikiDataLink(queryModel.Url) + ".";
            case "Country":
                searchWikiData = true;
                return " ?game wdt:P495 " + TransformWikiDataLink(queryModel.Url) + ".";
            case "DateMin":
                return " ?game wdt:P577 ?date. FILTER(YEAR(?date) > " + queryModel.Url + ")";
            case "DateMax":
                return " ?game wdt:P577 ?date. FILTER(YEAR(?date) < " + queryModel.Url + ")";
            default:
                return "";
        }
    }

    // Transform Resource Link into Query
    private string TransformWikiDataLink(string url)
    {
        string result = url.Split("/").Last();
        return "wd:" + result;
    }

    // Obtain and parse DbPedia Ranking Results
    public async Task<List<RankingModel>> QueryDbPedia(InterfaceManager interfaceManager)
    {
        string dbPediaQuery = string.Empty;

        foreach (Transform child in interfaceManager.queryContentHolder.transform)
        {
            dbPediaQuery += DBSearchQuery(child.gameObject.GetComponent<QueryModel>());
        }

        dbPediaQuery += " }";

        if (searchDbPedia)
        {
            if (await apiController.PerformDbPediaSearch(dbPediaQuery))
            {
                ParseQueryResults(apiController.dbPediaGames, dbRankingData);
                dbRankingData = CleanRankingData(dbRankingData);
            }

            return dbRankingData;
        }

        return null;
    }

    // Obtain and parse WikiData Ranking Results
    public async Task<List<string>> QueryWikiData(InterfaceManager interfaceManager)
    {
        string wikiDataQuery = string.Empty;

        foreach (Transform child in interfaceManager.queryContentHolder.transform)
        {
            wikiDataQuery += WikiDataSearchQuery(child.gameObject.GetComponent<QueryModel>());
        }

        wikiDataQuery += " FILTER (langMatches( lang(?name), \"EN\" ) ) }";

        if (searchWikiData)
        {
            if (await apiController.PerformWikiDataSearch(wikiDataQuery))
            {
                ParseWikiDataNames(apiController.wikiDataGames, wikiDataRankingData);
                wikiDataRankingData = CleanRankingData(wikiDataRankingData);
            }

            return wikiDataRankingData;
        }

        return null;
    }

    // Return true if Query contains parameters from both databases
    public bool CanJoinTables()
    {
        return searchDbPedia && searchWikiData;
    }
}
