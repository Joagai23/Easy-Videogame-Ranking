using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class ProgramManager : MonoBehaviour
{
    // Script References
    public InterfaceManager interfaceManager;
    public ParsingService parsingService;
    public APIController apiController;

    // Obtain and show DataBase Properties
    async void Start()
    {
        await LoadDataAsync();
    }

    public async Task LoadDataAsync()
    {
        interfaceManager.ActiveLoadingScreen();

        if (await apiController.FetchData())
        {
            interfaceManager.ActiveQueryScreen();
            parsingService.ParseParameters();
            interfaceManager.LoadPrefabLists();
        }
        else
        {
            interfaceManager.ActiveErrorScreen();
        }
    }

    // Obtain Rankings according to Parameters
    public async Task PerformSearchAsync()
    {
        try
        {
            interfaceManager.ActiveLoadingScreen();

            // Obtain DbPedia Data
            List<RankingModel> dbPediaDataList = await parsingService.QueryDbPedia(interfaceManager);

            // Obtain WikiData Data
            List<string> wikiDataList = await parsingService.QueryWikiData(interfaceManager);

            // Join Data by Name
            if (parsingService.CanJoinTables())
            {
                dbPediaDataList = (from data in dbPediaDataList where (wikiDataList.Contains(data.Name)) select data).ToList();
            }

            if (!dbPediaDataList.IsUnityNull() && dbPediaDataList.Count > 0)
            {
                // Show Ranking Data
                interfaceManager.LoadRankingList(dbPediaDataList);
                interfaceManager.ActiveRankingScreen();
            }
            else
            {
                // Show Empty Query Result
                interfaceManager.ActiveQueryError();
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            interfaceManager.ActiveErrorScreen();
        }
    } 
}
