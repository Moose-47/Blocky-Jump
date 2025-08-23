using PlayFab.ClientModels;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject leaderboardPanel;

    public TMP_Text finalScoreTxt;
    public TMP_Text highscoreTxt;

    private void OnEnable()
    {
        int score = GameManager.Instance._score;
        int highscore = GameManager.Instance.Highscore;

        finalScoreTxt.text = $"Score: {score}";
        highscoreTxt.text = $"Highscore: {highscore}";

        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("Highscore", highscore);
            PlayerPrefs.Save();

            highscoreTxt.text = $"Highscore: {highscore}";

            SubmitScore(highscore);
        }
    }
    private void SubmitScore(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new System.Collections.Generic.List<StatisticUpdate>
            {
                new StatisticUpdate {StatisticName = "Highscore", Value = score }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request,
            result => Debug.Log("Score submitted successfully"),
            error => Debug.LogError("Failed to submite score: " + error.GenerateErrorReport())
            );
    }

    public void PlayAgain() => SceneManager.LoadScene("GameScene");
    public void MainMenu() => SceneManager.LoadScene("Main Menu");
    public void OpenLeaderboard() => leaderboardPanel.SetActive(true);
    public void CloseLeaderboard() => leaderboardPanel.SetActive(false);
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
