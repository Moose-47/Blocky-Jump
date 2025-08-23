using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject pausePanel;
    public GameObject howToPlayPanel;
    public GameObject settingsPanel;

    public Button pauseButton;
    public Button backButton;

    private void Start()
    {
        pausePanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        settingsPanel.SetActive(false);

        if (pauseButton) pauseButton.onClick.AddListener(Pause);
        if (backButton) backButton.onClick.AddListener(CloseAllPanels);
    }
    private void OnDestroy()
    {
        pauseButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }

    private void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    
    public void Resume()
    {
        CloseAllPanels();
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

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

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
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
    }
}
