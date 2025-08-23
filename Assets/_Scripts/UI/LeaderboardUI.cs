using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform contentParent;
    public GameObject leaderboardEntryPrefab;

    private void OnEnable()
    {
        Getleaderboard();
    }

    public void Getleaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Highscore",
            StartPosition = 0,
        };

        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardSuccess, OnLeaderboardfailure);
    }

    private void OnLeaderboardSuccess(GetLeaderboardResult result)
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var entry in result.Leaderboard)
        {
            GameObject go = Instantiate(leaderboardEntryPrefab, contentParent);
            TMP_Text[] texts = go.GetComponentsInChildren<TMP_Text>();

            texts[0].text = entry.DisplayName;
            texts[1].text = entry.StatValue.ToString();
        }
    }
    private void OnLeaderboardfailure(PlayFabError error) => Debug.LogError("Failed to get leaderboard: " + error.GenerateErrorReport());   
}
