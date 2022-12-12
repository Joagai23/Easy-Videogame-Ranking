using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    // UI References
    public GameObject loadingScreen;
    public GameObject queryScreen;
    public GameObject rankingScreen;
    public GameObject errorText;
    public GameObject loadingWheel;
    public GameObject queryError;

    // Prefab References
    public GameObject queryPrefab;
    public GameObject parameterPrefab;
    public GameObject rankingPrefab;

    // Content Holder References
    public GameObject queryContentHolder;
    public List<GameObject> referenceContentHolder;
    public List<GameObject> tabContentHolder;
    public GameObject rankingContentHolder;

    // Color References
    public List<Color> colorList;

    // Script References
    public ParsingService parsingService;

    // Instantiate Property Objects according to their type
    public void LoadPrefabLists()
    {
        foreach (ParameterModel dbParameterData in parsingService.dbPediaPlatforms)
        {
            GameObject gameObject = Instantiate(parameterPrefab, referenceContentHolder[0].transform);
            ParameterModel gameObjectData = gameObject.GetComponent<ParameterModel>();
            gameObjectData.interfaceManager = this;
            gameObjectData.SetData(dbParameterData.Url, dbParameterData.Name, "Platform");
            gameObjectData.GetComponentInChildren<Image>().color = new Color(colorList[0].r, colorList[0].g, colorList[0].b, 1.0f);
        }
        foreach (ParameterModel dbParameterData in parsingService.dbPediaModes)
        {
            GameObject gameObject = Instantiate(parameterPrefab, referenceContentHolder[1].transform);
            ParameterModel gameObjectData = gameObject.GetComponent<ParameterModel>();
            gameObjectData.interfaceManager = this;
            gameObjectData.SetData(dbParameterData.Url, dbParameterData.Name, "Mode");
            gameObjectData.GetComponentInChildren<Image>().color = new Color(colorList[1].r, colorList[1].g, colorList[1].b, 1.0f);
        }
        foreach (ParameterModel dbParameterData in parsingService.dbPediaGenres)
        {
            GameObject gameObject = Instantiate(parameterPrefab, referenceContentHolder[2].transform);
            ParameterModel gameObjectData = gameObject.GetComponent<ParameterModel>();
            gameObjectData.interfaceManager = this;
            gameObjectData.SetData(dbParameterData.Url, dbParameterData.Name, "Genre");
            gameObjectData.GetComponentInChildren<Image>().color = new Color(colorList[2].r, colorList[2].g, colorList[2].b, 1.0f);
        }
        foreach (ParameterModel dbParameterData in parsingService.wikiDataRatings)
        {
            GameObject gameObject = Instantiate(parameterPrefab, referenceContentHolder[3].transform);
            ParameterModel gameObjectData = gameObject.GetComponent<ParameterModel>();
            gameObjectData.interfaceManager = this;
            gameObjectData.SetData(dbParameterData.Url, dbParameterData.Name, "Rating");
            gameObjectData.GetComponentInChildren<Image>().color = new Color(colorList[3].r, colorList[3].g, colorList[3].b, 1.0f);
        }
        foreach (ParameterModel dbParameterData in parsingService.wikiDataCountries)
        {
            GameObject gameObject = Instantiate(parameterPrefab, referenceContentHolder[4].transform);
            ParameterModel gameObjectData = gameObject.GetComponent<ParameterModel>();
            gameObjectData.interfaceManager = this;
            gameObjectData.SetData(dbParameterData.Url, dbParameterData.Name, "Country");
            gameObjectData.GetComponentInChildren<Image>().color = new Color(colorList[4].r, colorList[4].g, colorList[4].b, 1.0f);
        }

        HideContent();

        // Set Platform-Instances
        UnhideContent(string.Empty);
    }

    // Process and Instantiate Ranking Objects
    public void LoadRankingList(List<RankingModel> dbPediaDataList)
    {
        // Order Data by Score
        dbPediaDataList = dbPediaDataList.OrderByDescending(x => x.Score).ToList();

        // Obtain 100 Best
        dbPediaDataList = dbPediaDataList.Take(100).ToList();

        // Ranking Variables
        int currentPosScore = dbPediaDataList[0].Score;
        int currentPos = 1;

        // Show Data
        for (int i = 0; i < dbPediaDataList.Count; i++)
        {
            if (currentPosScore > dbPediaDataList[i].Score)
            {
                currentPos = (i + 1);
                currentPosScore = dbPediaDataList[i].Score;
            }

            GameObject gameObject = Instantiate(rankingPrefab, rankingContentHolder.transform);
            RankingModel gameObjectData = gameObject.GetComponent<RankingModel>();
            gameObjectData.SetData(dbPediaDataList[i].Name, dbPediaDataList[i].Series, dbPediaDataList[i].Publisher, dbPediaDataList[i].ReleaseDate, currentPosScore.ToString(), currentPos.ToString());
        }
    }

    // Instantiate Query Objects according to their type
    public void AddQueryProperty(ParameterModel parameter)
    {
        GameObject gameObject = Instantiate(queryPrefab, queryContentHolder.transform);
        QueryModel gameObjectData = gameObject.GetComponent<QueryModel>();
        gameObjectData.SetData(parameter.Url, parameter.Name, parameter.Type);

        switch (parameter.Type)
        {
            case "Mode":
                gameObjectData.GetComponentInChildren<Image>().color = new Color(colorList[1].r, colorList[1].g, colorList[1].b, 1.0f);
                break;
            case "Genre":
                gameObjectData.GetComponentInChildren<Image>().color = new Color(colorList[2].r, colorList[2].g, colorList[2].b, 1.0f);
                break;
            case "Rating":
                gameObjectData.GetComponentInChildren<Image>().color = new Color(colorList[3].r, colorList[3].g, colorList[3].b, 1.0f);
                break;
            case "Country":
                gameObjectData.GetComponentInChildren<Image>().color = new Color(colorList[4].r, colorList[4].g, colorList[4].b, 1.0f);
                break;
            case "DateMin":
                gameObjectData.GetComponentInChildren<Image>().color = new Color(colorList[5].r, colorList[5].g, colorList[5].b, 1.0f);
                break;
            case "DateMax":
                gameObjectData.GetComponentInChildren<Image>().color = new Color(colorList[5].r, colorList[5].g, colorList[5].b, 1.0f);
                break;
            default: // Platform
                gameObjectData.GetComponentInChildren<Image>().color = new Color(colorList[0].r, colorList[0].g, colorList[0].b, 1.0f);
                break;
        }
    }

    // Hide all tabs
    public void HideContent()
    {
        foreach (GameObject contentHolder in tabContentHolder)
        {
            contentHolder.SetActive(false);
        }
    }

    // Unhide one tab
    public void UnhideContent(string tabName)
    {
        switch (tabName)
        {
            case "ModeTab":
                tabContentHolder[1].SetActive(true);
                break;
            case "GenreTab":
                tabContentHolder[2].SetActive(true);
                break;
            case "AgeRatingTab":
                tabContentHolder[3].SetActive(true);
                break;
            case "OriginCountryTab":
                tabContentHolder[4].SetActive(true);
                break;
            case "ReleaseDateTab":
                tabContentHolder[5].SetActive(true);
                break;
            default:
                tabContentHolder[0].SetActive(true);
                break;
        }
    }

    // Show Loading Screen
    public void ActiveLoadingScreen()
    {
        loadingScreen.SetActive(true);
        loadingWheel.SetActive(true);
        queryScreen.SetActive(false);
        rankingScreen.SetActive(false);
        errorText.SetActive(false);
        queryError.SetActive(false);
    }

    // Show Query Screen 
    public void ActiveQueryScreen()
    {
        loadingScreen.SetActive(false);
        loadingWheel.SetActive(false);
        queryScreen.SetActive(true);
        rankingScreen.SetActive(false);
        errorText.SetActive(false);
        queryError.SetActive(false);
    }

    // Show Error Screen
    public void ActiveErrorScreen()
    {
        loadingScreen.SetActive(true);
        loadingWheel.SetActive(false);
        queryScreen.SetActive(false);
        rankingScreen.SetActive(false);
        errorText.SetActive(true);
        queryError.SetActive(false);
    }

    // Show Ranking Screen
    public void ActiveRankingScreen()
    {
        loadingScreen.SetActive(false);
        loadingWheel.SetActive(false);
        queryScreen.SetActive(false);
        rankingScreen.SetActive(true);
        errorText.SetActive(false);
        queryError.SetActive(false);
    }

    // Show Query Error
    public void ActiveQueryError()
    {
        loadingScreen.SetActive(true);
        loadingWheel.SetActive(false);
        queryScreen.SetActive(false);
        rankingScreen.SetActive(true);
        errorText.SetActive(false);
        queryError.SetActive(true);
    }

    // Destroy Ranking Screen and Show Query
    public void BackToQuery()
    {
        ActiveQueryScreen();

        foreach (Transform child in rankingContentHolder.transform)
        {
            Destroy(child.gameObject);
        }

        parsingService.CleanQueryResults();

        // Reset view to top of the list
        rankingContentHolder.GetComponent<RectTransform>().position = new Vector3(rankingContentHolder.transform.position.x, 0.0f, rankingContentHolder.transform.position.z);
    }
}
