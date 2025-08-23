using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject howToPlayPanel;
    public GameObject creditsPanel;
    public GameObject leaderboardPanel;

    [Header("Highscore Display")]
    public TMP_Text highscoreText;

    private void Start()
    {
        Time.timeScale = 1f;

        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        creditsPanel.SetActive(false);

        int highscore = PlayerPrefs.GetInt("Highscore", 0);
        highscoreText.text = "Highscore: " + highscore.ToString();
    }

    public void PlayGame() => SceneManager.LoadScene("GameScene");

    public void OpenSettings()
    {
        CloseAllPanels();
        settingsPanel.SetActive(true);
    }

    public void OpenHowToPlay()
    {
        CloseAllPanels();
        howToPlayPanel.SetActive(true);
    }
    public void OpenCredits()
    {
        CloseAllPanels();
        creditsPanel.SetActive(true);
    }

    public void OpenLeaderboard()
    {
        CloseAllPanels();
        leaderboardPanel.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void CloseAllPanels()
    {
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        creditsPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
    }
}
