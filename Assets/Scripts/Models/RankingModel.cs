using UnityEngine;
using UnityEngine.UI;

public class RankingModel : MonoBehaviour
{
    // Public class properties
    public string Resource;
    public string Name;
    public string Series;
    public string Publisher;
    public string ReleaseDate;
    public string ScoreString;
    public int Score;

    // UI References
    public Text NameText;
    public Text SeriesText;
    public Text PublisherText;
    public Text DateText;
    public Text RankText;
    public Text ScoreText;

    // Assign class properties
    public RankingModel(string resource, string name, string series, string publisher, string releaseDate, string score)
    {
        Resource = resource;
        Name = name;
        Series = series;
        Publisher = publisher;
        ReleaseDate = releaseDate;
        ScoreString = score;
    }

    // Update UI
    public void SetData(string name, string series, string publisher, string releaseDate, string score, string rank)
    {
        NameText.text = name;
        SeriesText.text = series;
        PublisherText.text = publisher;
        DateText.text = releaseDate;
        RankText.text = rank;
        ScoreText.text = score;
    }
}
