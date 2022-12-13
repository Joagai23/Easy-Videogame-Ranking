using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class APIController : MonoBehaviour
{
    // Public class properties
    public string dbPediaComputingPlatformResult = String.Empty;
    public string dbPediaGenreResult = String.Empty;
    public string dbPediaModeResult = String.Empty;
    public string wikiDataRatingResult = String.Empty;
    public string wikiDataCountriesResult = String.Empty;
    public string dbPediaGames = String.Empty;
    public string wikiDataGames = String.Empty;

    // Static URL and Queries: DBpedia
    private static readonly string dbPediaUrl = "https://dbpedia.org/sparql";
    private static readonly string dbPediaComputingPlatformQuery = "?query=SELECT DISTINCT ?resource ?name WHERE { ?game rdf:type dbo:VideoGame. ?game dbo:computingPlatform ?resource. ?resource dbp:title ?name. }";
    private static readonly string dbPediaGenresQuery = "?query=SELECT DISTINCT ?resource ?name WHERE { ?game rdf:type dbo:VideoGame. ?game dbp:genre ?resource . BIND('Name' AS ?name). }";
    private static readonly string dbPediaModesQuery = "?query=SELECT DISTINCT ?resource  ?name WHERE { ?game rdf:type dbo:VideoGame. ?game dbp:modes ?resource . BIND('Name' AS ?name). }";
    // Searching Query
    private string dbPediaVideogameQuery = "?query=SELECT DISTINCT ?game ?name ?series ?publisher ?releaseDate ?ign WHERE { ?game rdf:type dbo:VideoGame. ?game dbp:title ?name. ?game dbp:series ?series. ?game dbo:publisher ?publisher. ?game dbo:releaseDate ?releaseDate. ?game dbp:ign ?ign.";

    // Static URL and Queries: Wikidata
    private static readonly string wikiDataUrl = "https://query.wikidata.org/sparql";
    private static readonly string wikiDataRatingQuery = "?query=SELECT DISTINCT ?resource ?name WHERE { ?game wdt:P31 wd:Q7889 . ?game wdt:P852 ?resource . ?resource rdfs:label ?name . FILTER (langMatches( lang(?name), 'EN' ) ) }";
    private static readonly string wikiDataCountryQuery = "?query=SELECT DISTINCT ?resource ?name WHERE { ?game wdt:P31 wd:Q7889 . ?game wdt:P495 ?resource . ?resource rdfs:label ?name . FILTER (langMatches( lang(?name), 'EN' ) ) }";
    // Searching Query
    private string wikiDataVideogameQuery = "?query=SELECT DISTINCT ?name WHERE { ?game rdfs:label ?name.";

    // Obtain all necessary property data
    public async Task<bool> FetchData()
    {
        try
        {
            dbPediaComputingPlatformResult = await GetRequests(dbPediaUrl + dbPediaComputingPlatformQuery);
            dbPediaGenreResult = await GetRequests(dbPediaUrl + dbPediaGenresQuery);
            dbPediaModeResult = await GetRequests(dbPediaUrl + dbPediaModesQuery);

            wikiDataRatingResult = await GetRequests(wikiDataUrl + wikiDataRatingQuery);
            wikiDataCountriesResult = await GetRequests(wikiDataUrl + wikiDataCountryQuery);

            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }

    // Query DbPedia Parameters
    public async Task<bool> PerformDbPediaSearch(string extraQuery)
    {
        try
        {
            string dbPediaQuery = dbPediaVideogameQuery + extraQuery;
            dbPediaGames = await GetRequests(dbPediaUrl + dbPediaQuery);
            
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }

    // Query WikiData Parameters
    public async Task<bool> PerformWikiDataSearch(string extraQuery)
    {
        try
        {
            string wikiDataQuery = wikiDataVideogameQuery + extraQuery;
            wikiDataGames = await GetRequests(wikiDataUrl + wikiDataQuery);

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }

    // GET Request
    private async Task<string> GetRequests(string uri)
    {
        try
        {
            using var client = new HttpClient();

            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("EasyVideogameRanking", "1.0"));
            client.DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return await client.GetStringAsync(uri);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
