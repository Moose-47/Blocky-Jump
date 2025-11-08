using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject pausePanel;
    public GameObject howToPlayPanel;
    public GameObject settingsPanel;
    public GameObject powerUpsPanel;
    public GameObject gameOverPanel;
    public Button pauseButton;

    private void Start()
    {
        pauseButton.gameObject.SetActive(true);
        pausePanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        settingsPanel.SetActive(false);

        if (pauseButton) pauseButton.onClick.AddListener(Pause);
    }
    private void OnDestroy()
    {
        pauseButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        if (pausePanel.gameObject.activeSelf || gameOverPanel.gameObject.activeSelf) 
            powerUpsPanel.SetActive(false);
        else
            powerUpsPanel.SetActive(true);
    }

    private void Pause()
    {
        pausePanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }
    
    public void Resume()
    {
        CloseAllPanels();
        pausePanel.SetActive(false);
        pauseButton.gameObject.SetActive(true);
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
        GameManager.Instance._score = 0;
        SceneManager.LoadScene("Main Menu");
    }

    public void GiveUp()
    {
        GameManager.Instance.PlayerDeath();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void CloseAllPanels()
    {
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }
}
